using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Put
{
	public class PermissionPutRequest : BasePutRequest<Permission>
	{
		[Required]
		public string Codename { get; set; }

		[Required]
		public string HelpText { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Permission permission;

			using (var repository = new Repository<Permission>(context, false))
			{
				permission = repository.GetById(this.Id);
			}

			if (permission == null)
				return null;

			permission.Codename = this.Codename;
			permission.HelpText = this.HelpText;

			return permission;
		}

		public override void FromEntity(Entity entity)
		{
			var permission = (Permission)entity;

			this.Id = permission.Id;
			this.Codename = permission.Codename;
			this.HelpText = permission.HelpText;
		}
	}
}