using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public class GroupPostRequest : BasePostRequest<Group>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			return new Group()
			{
				Name = this.Name
			};
		}

		public override Task<Entity> ToEntityAsync(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override void FromEntity(Entity entity)
		{
			var group = (Group) entity;

			this.Name = group.Name;
		}
	}
}