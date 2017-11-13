using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class CredentialPutRequest : BasePutRequest<Credential>
	{
		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string PasswordConfirm { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Credential credential;

			using (var repository = new Repository<Credential>(context, false))
			{
				credential = repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.ApiKey)
					.SingleOrDefault();
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