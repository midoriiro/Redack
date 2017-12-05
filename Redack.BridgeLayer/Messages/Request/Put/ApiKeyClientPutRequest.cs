using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class ApiKeyClientPutRequest : ApiKeyPutRequest
	{
		[Required]
		public int Client { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
				client = await repository.GetByIdAsync(this.Client);

			return new ApiKey()
			{
				Id = this.Id,
				Key = this.Key,
				Client = client
			};
		}

		public override void FromEntity(Entity entity)
		{
			var apikey = (ApiKey) entity;

			this.Id = apikey.Id;
			this.Key = apikey.Key;
			this.Client = apikey.Client.Id;
		}
	}
}