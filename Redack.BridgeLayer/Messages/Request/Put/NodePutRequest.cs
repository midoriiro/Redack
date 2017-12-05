using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class NodePutRequest : BasePutRequest<Node>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Node node;

			using (var repository = new Repository<Node>(context, false))
				node = await repository.GetByIdAsync(this.Id);

			if (node == null)
				return null;

			return node;
		}

		public override void FromEntity(Entity entity)
		{
			var node = (Node) entity;

			this.Id = node.Id;
			this.Name = node.Name;
		}
	}
}