using System.ComponentModel.DataAnnotations;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public abstract class BasePutRequest<TEntity> : BasePostRequest<TEntity> where TEntity : Entity
	{
		[Required]
		public int Id { get; set; }
	}
}