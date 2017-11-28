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
	public class ClientPostRequest : BasePostRequest<Client>
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string PassPhrase { get; set; }

		[Required]
		public int ApiKey { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			ApiKey apikey;

			using (var repository = new Repository<ApiKey>(context, false))
				apikey = await repository
					.Query(e => e.Id == this.ApiKey)
					.SingleOrDefaultAsync();

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