using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Security;

namespace Redack.ServiceLayer.Filters
{
    public class JwtAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthenticationHeaderValue authorization = actionContext.Request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Basic")
            {
                this.Unauthorized(actionContext);
                return;
            }

            Identity identity = this.GetIdentity(authorization.Parameter);

            if(identity == null || !this.ValidIdentity(identity) || identity.Client.IsBlocked)
            {
                this.Unauthorized(actionContext);
                return;
            }

            JwtIdentity jwtIdentity = new JwtIdentity(identity);

            actionContext.RequestContext.Principal = jwtIdentity.GetPrincipal();
        }

        public void Unauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        public Identity GetIdentity(string token)
        {
            Identity identity;

            using (var repository = new Repository<Identity>())
            {
                identity = repository.Query(e => e.Access == token).Single();
            }

            return identity;
        }

        public bool ValidIdentity(Identity identity)
        {
            return JwtTokenizer.IsValid(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, identity.Access);
        }
    }
}