using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public class MessagePutRequest : BasePutRequest<Message>
	{
		[Required]
		public string Text { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new System.NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			Message message;

			using (var repository = new Repository<Message>(context, false))
			{
				message = await repository
					.Query(e => e.Id == this.Id)
					.Include(e => e.Thread)
					.Include(e => e.Author)
					.SingleOrDefaultAsync();
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