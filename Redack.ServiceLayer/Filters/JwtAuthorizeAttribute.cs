using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Controllers;
using Redack.ServiceLayer.Security;

namespace Redack.ServiceLayer.Filters
{
	public class JwtAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly string _permission;
		private readonly string _group;

		public JwtAuthorizeAttribute(string permission, string group = null)
		{
			this._permission = permission;
			this._group = group;
		}

		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			if (actionContext.RequestContext.Principal.Identity is JwtIdentity identity)
			{
				var user = identity.GetUser();
				var groups = user.Groups;

				if (this._group != null && 
					groups.Any(e => e.Name == this._group) && 
					groups.Any(e => e.Permissions.Any(p => p.ToString() == this._permission)))
				{
					return true;
				}

				if (this._group == null && user.Permissions.Any(e => e.ToString() == this._permission))
				{
					return true;
				}
			}

			return false;
		}

		public bool Control(HttpActionContext actionContext)
		{
			return this.IsAuthorized(actionContext);
		}
	}
}