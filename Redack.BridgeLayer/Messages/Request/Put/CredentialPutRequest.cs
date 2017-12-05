using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class CredentialPutRequest : BasePutRequest<Credential>
	{
		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string PasswordConfirm { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Credential credential;

			using (var repository = new Repository<Credential>(context, false))
			{
				credential = await repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.ApiKey)
					.SingleOrDefaultAsync();
			}

			if (credential == null)
				return null;

			credential.Login = this.Login;
			credential.Password = this.Password;
			credential.PasswordConfirm = this.PasswordConfirm;

			credential.ToHash();

			return credential;
		}

		public override void FromEntity(Entity entity)
		{
			var credential = (Credential) entity;

			this.Id = credential.Id;
			this.Login = credential.Login;
			this.Password = credential.Password;
			this.PasswordConfirm = credential.PasswordConfirm;
		}
	}
}