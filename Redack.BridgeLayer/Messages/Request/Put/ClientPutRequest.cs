using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class ClientPutRequest : BasePutRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
			{
				client = await repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.ApiKey)
					.SingleOrDefaultAsync();
			}

			if (client == null)
				return null;

			client.Name = this.Name;

			return client;
		}

		public override void FromEntity(Entity entity)
		{
			var client = (Client) entity;

			this.Id = client.Id;
			this.Name = client.Name;
		}
	}
}