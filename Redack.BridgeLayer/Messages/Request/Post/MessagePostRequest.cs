using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Post
{
	public class MessagePostRequest : BasePostRequest<Message>
	{
		[Required]
		public string Text { get; set; }

		[Required]
		public int Thread { get; set; }

		[Required]
		public int Author { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new System.NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Thread thread;

			using (var repository = new Repository<Thread>(context, false))
				thread = await repository.GetByIdAsync(this.Thread);

			User author;

			using (var repository = new Repository<User>(context, false))
				author = await repository.GetByIdAsync(this.Author);

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