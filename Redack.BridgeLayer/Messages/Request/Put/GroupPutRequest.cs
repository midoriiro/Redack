using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class GroupPutRequest : BasePutRequest<Group>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Group group;

			using (var repository = new Repository<Group>(context, false))
				group = await repository.GetByIdAsync(this.Id);

			if (group == null)
				return null;

			group.Name = this.Name;

			return group;
		}

		public override void FromEntity(Entity entity)
		{
			var group = (Group) entity;

			this.Id = group.Id;
			this.Name = group.Name;
		}
	}
}