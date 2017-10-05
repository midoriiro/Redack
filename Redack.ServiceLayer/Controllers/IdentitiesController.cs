using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Security;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/identities")]
    public class IdentitiesController : ApiController
    {
        private readonly RedackDbContext _context;
        private readonly IRepository<User> _repositoryUser;
        private readonly IRepository<Identity> _repository;

        protected IdentitiesController()
        {
            this._context = new RedackDbContext();
            this._repositoryUser = new Repository<User>(this._context);
            this._repository = new Repository<Identity>(this._context);
        }

        // POST: api/Identities/
        [HttpPost]
        [Route("signin")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignIn([FromBody]Credential request)
        {
            User user;

            try
            {
                user = this._repositoryUser
                    .Query(e => e.Credential.Login == request.Login && e.Credential.Password == request.Password).Single();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }

            Identity identity;

            try
            {
                identity = this._repository.Query(e => e.ApiKey.Id == user.Credential.Id).Single();
            }
            catch (InvalidOperationException)
            {
                identity = new Identity()
                {
                    ApiKey = user.Credential.ApiKey
                };
            }

            var tokenizer = new JwtTokenizer();
            var token = tokenizer.Encode(identity, 5);

            identity.Token = token;

            this._repository.InsertOrUpdate(identity);
            await this._repository.CommitAsync();

            return this.Ok(token);
        }

        [HttpPost]
        [Route("signup")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignUp([FromBody]User request)
        {
            request.Credential.ApiKey = new ApiKey()
            {
                Key = ApiKey.GenerateKey(256)
            };

            this._repositoryUser.Insert(request);

            try
            {
                await this._repositoryUser.CommitAsync();
            }
            catch (DbUpdateException)
            {
                if (this._repositoryUser.Query(e => e.Id == request.Id).Count() == 1)
                    return this.Conflict();

                throw;
            }

            return this.Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._repository.Dispose();
                this._repositoryUser.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}