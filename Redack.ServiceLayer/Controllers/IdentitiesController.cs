using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models;
using Redack.ServiceLayer.Security;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/identities")]
    public class IdentitiesController : ApiController
    {
        private readonly RedackDbContext _context;
        private readonly IRepository<User> _repositoryUser;
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<Identity> _repository;

        public IdentitiesController()
        {
            this._context = new RedackDbContext();
            this._repositoryUser = new Repository<User>(this._context);
            this._repositoryClient = new Repository<Client>(this._context);
            this._repository = new Repository<Identity>(this._context);
        }

        // POST: api/Identities/
        [HttpPost]
        [Route("signup")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignUp([FromBody]CredentialSignUpRequest request)
        {
            try
            {
                var client = this._repositoryClient.Query(e => e.Key.Id == request.Client.Key.Id).Single();

                if (client.IsBlocked)
                    throw new InvalidOperationException();

            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }

            var user = DomainLayer.Model.User.Create(
                request.Login, request.Password, request.PasswordConfirm);

            this._repositoryUser.Insert(user);

            try
            {
                await this._repositoryUser.CommitAsync();
            }
            catch (DbEntityValidationException)
            {
                return this.BadRequest(this.ModelState);
            }
            catch (DbUpdateException)
            {
                if (this._repositoryUser.Query(e => e.Credential.Login == request.Login).Count() == 1)
                    return this.Conflict();

                throw;
            }

            return this.CreatedAtRoute(WebApiConfig.DefaultRouteName, new { id = user.Id }, user);
        }

        // POST: api/Identities/
        [HttpPost]
        [Route("signin")]
        [ResponseType(typeof(TokenResponse))]
        public virtual async Task<IHttpActionResult> SignIn([FromBody]CredentialSignInRequest request)
        {
            User user;
            Client client;

            try
            {
                user = this._repositoryUser.Query(
                    e => e.Credential.Login == request.Login && e.Credential.Password == request.Password).Single();

                client = this._repositoryClient.Query(e => e.Key.Id == request.Client.Key.Id).Single();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }

            Identity identity;

            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                this._repository.Query(
                    e => e.ApiKey.Id == user.Credential.ApiKey.Id && e.Client.Id == request.Client.Id).Single();

                return this.Conflict();
            }
            catch (InvalidOperationException)
            {
                identity = new Identity
                {
                    Client = client,
                    ApiKey = user.Credential.ApiKey
                };
            }

            var tokenizer = new JwtTokenizer();
            var tokenAccess = tokenizer.Encode(identity, 5);
            var tokenRefresh = tokenizer.Encode(identity, 604800);

            
            identity.Access = tokenAccess;
            identity.Refresh = tokenRefresh;

            this._repository.Insert(identity);
            await this._repository.CommitAsync();

            return this.Ok(new TokenResponse(){Access = tokenAccess, Refresh = tokenRefresh });
        }

        // POST: api/Identities/
        [JwtAuthorizationFilter]
        [HttpGet]
        [Route("signout")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignOut()
        {
            var jwtIdentity = this.User.Identity as JwtIdentity;

            Identity identity;
            
            try
            {
                identity = jwtIdentity.GetIdentity();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }

            this._repository.Delete(identity);
            await this._repository.CommitAsync();

            return this.Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._repository.Dispose();
                this._repositoryUser.Dispose();
                this._repositoryClient.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}