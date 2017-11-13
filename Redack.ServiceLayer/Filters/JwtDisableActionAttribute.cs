using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Redack.ServiceLayer.Filters
{
	public class JwtDisableActionAttribute : ActionFilterAttribute
	{
		private readonly bool _disabled;

		public JwtDisableActionAttribute(bool disabled = true)
		{
			this._disabled = disabled;
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if(this._disabled)
				actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed);

			base.OnActionExecuting(actionContext);
		}
	}
}