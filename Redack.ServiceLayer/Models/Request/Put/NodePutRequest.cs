using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Put
{
	public class NodePutRequest : BasePutRequest<Node>
	{
		[Required]
		public string Name { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Node node;

			using (var repository = new Repository<Node>(context, false))
			{
				node = repository.GetById(this.Id);
			}

			if (node == null)
				return null;

			return node;
		}

		public override void FromEntity(Entity entity)
		{
			var node = (Node) entity;

			this.Id = node.Id;
			this.Name = node.Name;
		}
	}
}