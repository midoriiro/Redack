using System;
using System.Data.Entity;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Security;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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
				identity = repository
					.All()
					.Where(e => e.Access == token)
					.Include(e => e.User.Credential.ApiKey)
					.Include(e => e.Client.ApiKey)
					.SingleOrDefault();
			}

			return identity;
		}

		public bool ValidIdentity(Identity identity)
		{
			return JwtTokenizer.IsValid(identity.User.Credential.ApiKey.Key, identity.Client.ApiKey.Key, identity.Access);
		}
	}
}