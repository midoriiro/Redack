using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class GroupPutRequest : BasePutRequest<Group>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Group group;

			using (var repository = new Repository<Group>(context, false))
			{
				group = repository.GetById(this.Id);
			}

			if (group == null)
				return null;

			group.Name = this.Name;

			return group;
		}

		public override void FromEntity(Entity entity)
		{
			var group = (Group) entity;

			this.Id = group.Id;
			this.Name = group.Name;
		}
	}
}