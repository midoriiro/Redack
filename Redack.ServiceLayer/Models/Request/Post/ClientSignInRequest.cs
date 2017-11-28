using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Data.Entity;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public class ClientSignInRequest : BasePostRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string PassPhrase { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
			{
				client = await repository
					.Query(e => e.Name == this.Name)
					.SingleOrDefaultAsync();

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