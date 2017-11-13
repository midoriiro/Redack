using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Redack.ServiceLayer.Controllers;

namespace Redack.ServiceLayer.Filters
{
	public class JwtAuthorizeOwnerAttribute : ActionFilterAttribute
	{
		private readonly string _resouceIdentifier;

		public JwtAuthorizeOwnerAttribute(string resouceIdentifier)
		{
			this._resouceIdentifier = resouceIdentifier;
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (!actionContext.ActionArguments.ContainsKey(this._resouceIdentifier))
				throw new ArgumentException(
					"Wrong resouce identifier specified or " +
					"action method signature does not have a resource identifier");

			var id = (int)actionContext.ActionArguments[this._resouceIdentifier];
			var controller = (IOwnerFilter)actionContext.ControllerContext.Controller;

			try
			{
				if (!controller.IsOwner(id))
					this.Unauthorized(actionContext);
			}
			catch (InvalidOperationException)
			{
				this.Unauthorized(actionContext);
			}

			base.OnActionExecuting(actionContext);
		}

		public void Unauthorized(HttpActionContext actionContext)
		{
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
		}
	}
}