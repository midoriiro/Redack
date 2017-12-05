using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Post
{
	public class MessageRevisionPostRequest : BasePostRequest<MessageRevision>
	{
		[Required]
		public int Editor { get; set; }

		[Required]
		public int Message { get; set; }

		public override Entity ToEntity(IDbContext context)
		{
			throw new NotImplementedException();
		}

		public override async Task<Entity> ToEntityAsync(IDbContext context)
		{
			User editor;

			using (var repository = new Repository<User>(context, false))
			{
				editor = await repository
					.Query(e => e.Id == this.Editor)
					.Include(e => e.Credential)
					.SingleOrDefaultAsync();
			}

			Message message;

			using (var repository = new Repository<Message>(context, false))
			{
				message = await repository
					.Query(e => e.Id == this.Message)
					.Include(e => e.Author)
					.Include(e => e.Thread)
					.SingleOrDefaultAsync();
			}

			return new MessageRevision()
			{
				Editor = editor,
				Message = message
			};
		}

		public override void FromEntity(Entity entity)
		{
			var revision = (MessageRevision) entity;

			this.Editor = revision.Editor.Id;
			this.Message = revision.Message.Id;
		}
	}
}