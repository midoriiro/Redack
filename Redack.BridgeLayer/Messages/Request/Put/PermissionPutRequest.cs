using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class PermissionPutRequest : BasePutRequest<Permission>
	{
		[Required]
		public string Codename { get; set; }

		[Required]
		public string HelpText { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Permission permission;

			using (var repository = new Repository<Permission>(context, false))
				permission = await repository.GetByIdAsync(this.Id);

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