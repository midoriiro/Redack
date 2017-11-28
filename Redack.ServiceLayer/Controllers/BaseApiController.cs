using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Security;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Redack.ServiceLayer.Controllers
{
	public class BaseApiController : ApiController
	{
		public IDbContext Context { get; private set; }

		public BaseApiController(IDbContext context) : base()
		{
			this.Context = context;
		}

		public Identity GetIdentity()
		{
			var jwtIdentity = (JwtIdentity)this.User.Identity;

			if (jwtIdentity == null)
				return null;

			Identity identity = jwtIdentity.Identity;

			if (identity == null)
				return null;

			return identity;
		}

		public string GetControllerRouteName()
		{
			return this.ActionContext.RequestContext.RouteData.Route.RouteTemplate.Split('/')[1];
		}
	}
}