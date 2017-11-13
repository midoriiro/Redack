using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class ApiKeyCredentialPutRequest : ApiKeyPutRequest
	{
		[Required]
		public int Credential { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Credential credential;

			using (var repository = new Repository<Credential>(context, false))
				credential = repository.GetById(this.Credential);

			return new ApiKey()
			{
				Id = this.Id,
				Key = this.Key,
				Credential = credential
			};
		}

		public override void FromEntity(Entity entity)
		{
			var apikey = (ApiKey) entity;

			this.Id = apikey.Id;
			this.Key = apikey.Key;
			this.Credential = apikey.Credential.Id;
		}
	}
}