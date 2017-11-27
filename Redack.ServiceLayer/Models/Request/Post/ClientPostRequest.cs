using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public class ClientPostRequest : BasePostRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string PassPhrase { get; set; }

		[Required]
		public int ApiKey { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			ApiKey apikey;

			using (var repository = new Repository<ApiKey>(context, false))
				apikey = repository
					.Query(e => e.Id == this.ApiKey)
					.SingleOrDefault();

			return Client.Create(this.Name, this.PassPhrase, apikey);
		}

		public override void FromEntity(Entity entity)
		{
			var client = (Client) entity;

			this.Name = client.Name;
			this.PassPhrase = client.PassPhrase;
			this.ApiKey = client.ApiKey.Id;
		}
	}
}