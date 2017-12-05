using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class UserPutRequest : BasePutRequest<User>
	{
		[Required]
		public string Alias { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			User user;

			using (var repository = new Repository<User>(context, false))
			{
				user = await repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.Credential)
					.SingleOrDefaultAsync();
			}

			if (user == null)
				return null;

			user.Alias = this.Alias;

			return user;
		}

		public override void FromEntity(Entity entity)
		{
			var user = (User) entity;

			this.Id = user.Id;
			this.Alias = user.Alias;
		}
	}
}