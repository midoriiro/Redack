using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class MessagePutRequest : BasePutRequest<Message>
	{
		[Required]
		public string Text { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Message message;

			using (var repository = new Repository<Message>(context, false))
			{
				message = repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.Thread)
					.Include(e => e.Author)
					.SingleOrDefault();
			}

			if (message == null)
				return null;

			message.Text = this.Text;

			return message;
		}

		public override void FromEntity(Entity entity)
		{
			var message = (Message) entity;

			this.Id = message.Id;
			this.Text = message.Text;
		}
	}
}