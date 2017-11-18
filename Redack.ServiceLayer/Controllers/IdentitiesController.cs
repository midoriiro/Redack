using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models;
using Redack.ServiceLayer.Models.Request;
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
        public static readonly int KeySize = 256;
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
        public virtual async Task<IHttpActionResult> SignUp([FromBody] SignUpRequest request)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            Identity identity = (Identity)request.ToEntity(this.Context);

            if(identity.Client == null || identity.Client.IsBlocked)
                return this.Unauthorized();

            this._repositoryUser.Insert(identity.User);

            try
            {
                await this._repositoryUser.CommitAsync();
            }
            catch (DbEntityValidationException e)
            {
                this.Validate<User>(identity.User);
                return this.BadRequest(this.ModelState);
            }
            catch (DbUpdateException)
            {
                return this.Conflict();
            }

            return this.CreatedAtRoute(
                WebApiConfig.DefaultRouteName, 
                new {
                    controller = "users",
                    id = identity.User.Id
                }, 
                identity.User);
        }

        // GET: api/Identities/
        [JwtAuthorizationFilter]
        [HttpGet]
        [Route("signdown")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> SignDown()
        {
            Identity identity = identity = this.GetIdentity();

            if (identity == null)
                return this.Unauthorized();

            await this.SignOutAll();

            this._repositoryUser.Delete(identity.User);
            await this._repositoryUser.CommitAsync();

            return this.Ok();
        }

        // POST: api/Identities/
        [HttpPost]
        [Route("signin")]
        [ResponseType(typeof(TokenResponse))]
        public virtual async Task<IHttpActionResult> SignIn([FromBody] SignInRequest request)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var identity = (Identity)request.ToEntity(this.Context);

            if (identity.User == null || identity.Client == null || identity.Client.IsBlocked)
                return this.Unauthorized();

            bool exist = this._repository
                .All()
                .Any(e => e.User.Id == identity.User.Id && e.Client.Id == identity.Client.Id);

            if(exist)
                return this.Conflict();

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
            if(!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            Identity identity = (Identity)request.ToEntity(this.Context);

            if (identity == null || identity.Client == null || identity.Client.IsBlocked)
                return this.Unauthorized();

            this._repositoryUser.Update(identity.User);

            try
            {
                await this._repositoryUser.CommitAsync();
            }
            catch (DbEntityValidationException)
            {
                this.Validate<User>(identity.User);
                return this.BadRequest(this.ModelState);
            }

            var identities = this._repository.Query(e => e.User.Id == identity.User.Id).ToList();
            identities.ForEach(e => this._repository.Delete(e));

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
            Identity identity = this.GetIdentity();

            if (identity == null)
                return this.Unauthorized();

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
            Identity identity = this.GetIdentity();

            if (identity == null)
                return this.Unauthorized();

            var identities = this._repository.Query(e => e.User.Id == identity.User.Id).ToList();
            identities.ForEach(e => this._repository.Delete(e));

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
            Identity identity = this.GetIdentity();

            if (identity == null || identity.Client.IsBlocked)
                return this.Unauthorized();

            if (!JwtTokenizer.IsValid(
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