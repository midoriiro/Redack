using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Post
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