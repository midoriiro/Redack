using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Post
{
	public class PermissionPostRequest : BasePostRequest<Permission>
	{
		[Required]
		public string Codename { get; set; }

		[Required]
		public string HelpText { get; set; }

		[Required]
		public string ContentType { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			return new Permission()
			{
				Codename = this.Codename,
				HelpText = this.HelpText,
				ContentType = this.ContentType
			};
		}

		public override Task<Entity> ToEntityAsync(IDbContext context)
		{
			throw new NotImplementedException();
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