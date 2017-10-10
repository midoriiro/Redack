using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.ServiceLayer.Security;
using Thread = System.Threading.Thread;

namespace Redack.ServiceLayer.Filters
{
    public class JwtAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = "";

            var authorization = actionContext.Request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Basic")
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            token = authorization.Parameter;

            Identity identity;

            using (var repository = new Repository<Identity>())
            {
                identity = repository.Query(e => e.Access == token).Single();
            }

            if(identity == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            var tokenizer = new JwtTokenizer();
            var tokenized = tokenizer.Decode(identity);

            // TODO : token validation

            User user;

            using (var repository = new Repository<User>())
            {
                user = repository.Query(e => e.Credential.ApiKey.Equals(identity.ApiKey)).Single();
            }

            if(user == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            IIdentity jwtIdentity = new JwtIdentity(user);
            IPrincipal principal = new GenericPrincipal(jwtIdentity, new string[]{});

            HttpContext.Current.User = principal;
        }
    }
}