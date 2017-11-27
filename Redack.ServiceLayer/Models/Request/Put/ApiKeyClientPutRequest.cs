using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Put
{
	public class ApiKeyClientPutRequest : ApiKeyPutRequest
	{
		[Required]
		public int Client { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
				client = repository.GetById(this.Client);

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