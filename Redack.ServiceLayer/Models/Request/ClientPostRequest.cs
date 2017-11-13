using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class ClientPostRequest : BaseRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public int ApiKey { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			ApiKey apikey;

			using (var repository = new Repository<ApiKey>(context, false))
				apikey = repository
					.Query(e => e.Id == this.ApiKey)
					.SingleOrDefault();

			return new Client()
			{
				Name = this.Name,
				ApiKey = apikey
			};
		}

		public override void FromEntity(Entity entity)
		{
			var client = (Client) entity;

			this.Name = client.Name;
			this.ApiKey = client.ApiKey.Id;
		}
	}
}