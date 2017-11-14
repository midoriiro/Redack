using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class PermissionPostRequest : BaseRequest<Permission>
	{
		[Required]
		public string Codename { get; set; }

		[Required]
		public string HelpText { get; set; }

		[Required]
		public string ContentType { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			return new Permission()
			{
				Codename = this.Codename,
				HelpText = this.HelpText,
				ContentType = this.ContentType
			};
		}

		public override void FromEntity(Entity entity)
		{
			var permission = (Permission)entity;

			this.Codename = permission.Codename;
			this.HelpText = permission.HelpText;
			this.ContentType = permission.ContentType;
		}
	}
}