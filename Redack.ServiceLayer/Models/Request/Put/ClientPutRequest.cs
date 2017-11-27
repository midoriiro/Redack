using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Put
{
	public class ClientPutRequest : BasePutRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
			{
				client = repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.ApiKey)
					.SingleOrDefault();
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