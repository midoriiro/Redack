using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Post
{
	public class SignUpRequest : BasePostRequest<Identity>
	{
		[Required]
		public int Client { get; set; }

		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string PasswordConfirm { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new System.NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			User user = User.Create(
				this.Login,
				this.Password,
				this.PasswordConfirm,
				ApiKey.KeySize);

			Client client;

			using (var repository = new Repository<Client>(context, false))
				client = await repository
					.Query(e => e.Id == this.Client)
					.SingleOrDefaultAsync();

			return new Identity()
			{
				Client = client,
				User = user,
			};
		}

		public override void FromEntity(Entity entity)
		{
			throw new System.NotImplementedException();
		}
	}
}