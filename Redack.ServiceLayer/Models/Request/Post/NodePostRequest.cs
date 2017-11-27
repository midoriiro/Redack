using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public class NodePostRequest : BasePostRequest<Node>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			return new Node()
			{
				Name = this.Name
			};
		}

		public override void FromEntity(Entity entity)
		{
			var node = (Node) entity;

			this.Name = node.Name;
		}
	}
}