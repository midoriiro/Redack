using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class ClientSignInRequest : BaseRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string PassPhrase { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
			{
				client = repository
					.Query(e => e.Name == this.Name)
					.SingleOrDefault();

				if (client == null || !client.IsValid(this.PassPhrase))
					return null;
			}

			return client;
		}

		public override void FromEntity(Entity entity)
		{
			var client = (Client) entity;

			this.Name = client.Name;
			this.PassPhrase = client.PassPhrase;
		}
	}
}