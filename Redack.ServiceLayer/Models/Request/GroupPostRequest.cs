using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class GroupPostRequest : BaseRequest<Group>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			return new Group()
			{
				Name = this.Name
			};
		}

		public override void FromEntity(Entity entity)
		{
			var group = (Group) entity;

			this.Name = group.Name;
		}
	}
}