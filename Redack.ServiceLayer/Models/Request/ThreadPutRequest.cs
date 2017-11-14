using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Redack.ServiceLayer.Models.Request
{
	public class ThreadPutRequest : BasePutRequest<Thread>
	{
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Thread thread;

			using (var repository = new Repository<Thread>(context, false))
			{
				thread = repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.Node)
					.SingleOrDefault();
			}

			if (thread == null)
				return null;

			thread.Title = this.Title;
			thread.Description = this.Description;

			return thread;
		}

		public override void FromEntity(Entity entity)
		{
			var thread = (Thread) entity;

			this.Id = thread.Id;
			this.Title = thread.Title;
			this.Description = thread.Description;
		}
	}
}