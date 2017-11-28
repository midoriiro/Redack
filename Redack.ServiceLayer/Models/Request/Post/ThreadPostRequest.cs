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
	public class ThreadPostRequest : BasePostRequest<Thread>
	{
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		public int Node { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Node node;

			using (var repository = new Repository<Node>(context, false))
			{
				node = await repository.GetByIdAsync(this.Node);
			}

			return new Thread()
			{
				Title = this.Title,
				Description = this.Description,
				Node = node
			};
		}

		public override void FromEntity(Entity entity)
		{
			var thread = (Thread) entity;

			this.Title = thread.Title;
			this.Description = thread.Description;
			this.Node = thread.Node.Id;
		}
	}
}