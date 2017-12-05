using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class ApiKeyCredentialPutRequest : ApiKeyPutRequest
	{
		[Required]
		public int Credential { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();

		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Credential credential;

			using (var repository = new Repository<Credential>(context, false))
				credential = await repository.GetByIdAsync(this.Credential);

			return new ApiKey()
			{
				Id = this.Id,
				Key = this.Key,
				Credential = credential
			};
		}

		public override void FromEntity(Entity entity)
		{
			var apikey = (ApiKey) entity;

			this.Id = apikey.Id;
			this.Key = apikey.Key;
			this.Credential = apikey.Credential.Id;
		}
	}
}