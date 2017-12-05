using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Post
{
	public class ApiKeyPostRequest : BasePostRequest<ApiKey>
	{
		[Required]
		public string Key { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			return new ApiKey
			{
				Key = this.Key
			};
		}

		public override Task<Entity> ToEntityAsync(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override void FromEntity(Entity entity)
		{
			var apikey = (ApiKey)entity;

			this.Key = apikey.Key;
		}
	}
}