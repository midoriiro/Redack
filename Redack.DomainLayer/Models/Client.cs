using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;

namespace Redack.DomainLayer.Models
{
	public class Client : Entity
	{
		[Required]
		[MaxLength(50, ErrorMessage = "Type less than 50 characters")]
		[MinLength(5, ErrorMessage = "Type at least 5 characters")]
		[Index(IsUnique = true)]
		public string Name { get; set; }

		[Required(ErrorMessage = "The passphrase field is required")]
		[MinLength(15, ErrorMessage = "Type at least 15 characters")]
		public string PassPhrase { get; set; }

		[Required(ErrorMessage = "The salt field is required")]
		public string Salt { get; set; }

		[Required]
		public bool IsBlocked { get; set; }

		// Navigation properties
		[Required]
		public virtual ApiKey ApiKey { get; set; }

		public virtual ICollection<Identity> Identities { get; set; }

		public Client()
		{
			this.IsBlocked = true;
		}

		public static Client Create(string name, string passphrase, ApiKey apikey)
		{
			var client = new Client() {
				Name = name,
				PassPhrase = passphrase,
				ApiKey = apikey
			};

			client.ToHash();

			return client;
		}

		public static byte[] CreateRandomSalt()
		{
			byte[] salt = new byte[256 / 8];

			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			provider.GetBytes(salt);

			return salt;
		}

		public void ToHash()
		{
			this.Salt = Convert.ToBase64String(Client.CreateRandomSalt());

			this.PassPhrase = Credential.ToHash(this.PassPhrase, Convert.FromBase64String(this.Salt));
		}

		public bool IsValid(string passphrase)
		{
			//passphrase = Credential.ToHash(passphrase, Convert.FromBase64String(this.Salt));

			return this.PassPhrase == passphrase;
		}

		public override IQueryable<Entity> Filter(IQueryable<Entity> query)
		{
			throw new NotImplementedException();
		}

		public override List<Entity> Delete()
		{
			throw new NotImplementedException();
		}
	}
}
