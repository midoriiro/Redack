using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public class MessagePostRequest : BaseRequest<Message>
	{
		[Required]
		public string Text { get; set; }

		[Required]
		public int Thread { get; set; }

		[Required]
		public int Author { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			Thread thread;

			using (var repository = new Repository<Thread>(context, false))
				thread = repository.GetById(this.Thread);

			User author;

			using (var repository = new Repository<User>(context, false))
				author = repository.GetById(this.Author);

			return new Message()
			{
				Author = author,
				Thread = thread,
				Text = this.Text
			};
		}

		public override void FromEntity(Entity entity)
		{
			var message = (Message) entity;

			this.Author = message.Author.Id;
			this.Thread = message.Thread.Id;
			this.Text = message.Text;
		}
	}
}