using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models;
using Redack.ServiceLayer.Security;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/identities")]
    public class IdentitiesController : BaseApiController
    {
        private readonly int _keySize = 256;
        private readonly int _expirationTimeAccess = 5;
        private readonly int _expirationTimeRefresh = 604800;

        private readonly IRepository<User> _repositoryUser;
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<Identity> _repository;

        public IdentitiesController()
        {
            this._repositoryUser = new Repository<User>(this.Context);
            this._repositoryClient = new Repository<Client>(this.Context);
            this._repository = new Repository<Identity>(this.Context);
        }

        // POST: api/Identities/
        [HttpPost]
        [Route("signup")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignUp([FromBody]SignUpRequest request)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                this._repositoryClient.Query(
                    e => e.ApiKey.Id == request.Client.ApiKey.Id && e.IsBlocked == false).Single();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }

            var user = DomainLayer.Models.User.Create(
                request.Login, 
                request.Password, 
                request.PasswordConfirm, 
                this._keySize);

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

        // GET: api/Identities/
        [JwtAuthorizationFilter]
        [HttpGet]
        [Route("signdown")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignDown()
        {
            Identity identity;

            try
            {
                identity = this.GetIdentity();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }

            this._repositoryUser.Delete(identity.User);
            await this._repositoryUser.CommitAsync();

            return this.Ok();
        }

        // POST: api/Identities/
        [HttpPost]
        [Route("signin")]
        [ResponseType(typeof(TokenResponse))]
        public virtual async Task<IHttpActionResult> SignIn([FromBody]SignInRequest request)
        {
            User user;
            Client client;

            try
            {
                user = this._repositoryUser.Query(
                    e => e.Credential.Login == request.Login && e.Credential.Password == request.Password).Single();

                client = this._repositoryClient.Query(
                    e => e.ApiKey.Id == request.Client.ApiKey.Id && e.IsBlocked == false).Single();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }

            var exist = this._repository.Query(
                    e => e.User.Credential.ApiKey.Id == user.Credential.ApiKey.Id &&
                         e.Client.Id == request.Client.Id)
                .Any();

            if(exist)
                return this.Conflict();

            Identity identity = new Identity
            {
                Client = client,
                User = user
            };

            this.GetToken(identity);

            this._repository.Insert(identity);
            await this._repository.CommitAsync();

            return this.Ok(new TokenResponse{Access = identity.Access, Refresh = identity.Refresh });
        }

        // GET: api/Identities/
        [HttpPost]
        [Route("signin/forgotpassword")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignInForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            User user;

            try
            {
                user = this._repositoryUser.Query(
                    e => e.Credential.Login == request.Login && e.Credential.Password == request.OldPassword).Single();

                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                this._repositoryClient.Query(
                    e => e.ApiKey.Id == request.Client.ApiKey.Id && e.IsBlocked == false).Single();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }

            user = DomainLayer.Models.User.Update(
                user, 
                request.NewPassword, 
                request.NewPasswordConfirm, 
                this._keySize);

            this._repositoryUser.Update(user);

            try
            {
                await this._repositoryUser.CommitAsync();
            }
            catch (DbEntityValidationException)
            {
                return this.BadRequest(this.ModelState);
            }

            var identities = this._repository.Query(e => e.User.Id == user.Id).ToList();

            foreach (var id in identities)
            {
                this._repository.Delete(id);
            }

            await this._repository.CommitAsync();

            return this.Ok();
        }

        // GET: api/Identities/
        [JwtAuthorizationFilter]
        [HttpGet]
        [Route("signout")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignOut()
        {
            Identity identity;

            try
            {
                identity = this.GetIdentity();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }

            this._repository.Delete(identity);
            await this._repository.CommitAsync();

            return this.Ok();
        }

        // GET: api/Identities/
        [JwtAuthorizationFilter]
        [HttpGet]
        [Route("signout/all")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignOutAll()
        {
            Identity identity;

            try
            {
                identity = this.GetIdentity();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }

            var identities = this._repository.Query(e => e.User.Id == identity.User.Id).ToList();

            foreach (var id in identities)
            {
                this._repository.Delete(id);
            }

            await this._repository.CommitAsync();

            return this.Ok();
        }

        // GET: api/Identities/
        [JwtAuthorizationFilter]
        [HttpGet]
        [Route("refresh")]
        [ResponseType(typeof(TokenResponse))]
        public virtual async Task<IHttpActionResult> Refresh()
        {
            Identity identity;

            try
            {
                identity = this.GetIdentity();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }

            if (identity.Client.IsBlocked || 
                !JwtTokenizer.IsValid(
                    identity.User.Credential.ApiKey.Key, 
                    identity.Client.ApiKey.Key, 
                    identity.Refresh))
                return this.Unauthorized();

            this.GetToken(identity);

            this._repository.Update(identity);
            await this._repository.CommitAsync();

            return this.Ok(new TokenResponse { Access = identity.Access, Refresh = identity.Refresh });
        }

        protected void GetToken(Identity identity)
        {
            identity.Access = JwtTokenizer.Encode(
                identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, this._expirationTimeAccess);

            identity.Refresh = JwtTokenizer.Encode(
                identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, this._expirationTimeRefresh);
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