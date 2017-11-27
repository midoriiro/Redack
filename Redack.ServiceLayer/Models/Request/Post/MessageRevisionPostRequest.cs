using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public class MessageRevisionPostRequest : BasePostRequest<MessageRevision>
	{
		[Required]
		public int Editor { get; set; }

		[Required]
		public int Message { get; set; }

		public override Entity ToEntity(RedackDbContext context)
		{
			User editor;

			using (var repository = new Repository<User>(context, false))
			{
				editor = repository
					.Query(e => e.Id == this.Editor)
					.Include(e => e.Credential)
					.SingleOrDefault();
			}

			Message message;

			using (var repository = new Repository<Message>(context, false))
			{
				message = repository
					.Query(e => e.Id == this.Message)
					.Include(e => e.Author)
					.Include(e => e.Thread)
					.SingleOrDefault();
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