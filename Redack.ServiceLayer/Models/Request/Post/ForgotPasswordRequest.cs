using Redack.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;
using Redack.DatabaseLayer.DataAccess;
using System.Linq;
using Redack.ServiceLayer.Controllers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public class ForgotPasswordRequest : BasePostRequest<Identity>
	{
		[Required]
		public int Client { get; set; }

		[Required]
		public string Login { get; set; }

		[Required]
		public string OldPassword { get; set; }

		[Required]
		public string NewPassword { get; set; }

		[Required]
		public string NewPasswordConfirm { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new System.NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Client client;

			using (var repository = new Repository<Client>(context, false))
				client = await repository
					.Query(e => e.Id == this.Client)
					.SingleOrDefaultAsync();

			User user;

			using (var repository = new Repository<User>(context, false))
				user = await repository
					.Query(e => e.Credential.Login == this.Login && e.Credential.Password == this.OldPassword)
					.SingleOrDefaultAsync();

			if (user == null)
				return null;

			user = User.Update(
				user,
				this.NewPassword,
				this.NewPasswordConfirm,
				ApiKey.KeySize);

			return new Identity()
			{
				Client = client,
				User = user
			};
		}

		public override void FromEntity(Entity entity)
		{
			throw new System.NotImplementedException();
		}
	}
}