using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class ApiKeyPostRequest : BaseRequest<ApiKey>
	{
		[Required]
		public string Key { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			return new ApiKey
			{
				Key = this.Key
			};
		}

		public override void FromEntity(Entity entity)
		{
			var apikey = (ApiKey)entity;

			this.Key = apikey.Key;
		}		
	}
}