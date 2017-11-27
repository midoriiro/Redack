using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Models.Request.Post;
using System.ComponentModel.DataAnnotations;

namespace Redack.ServiceLayer.Models.Request.Put
{
	public abstract class BasePutRequest<TEntity> : BasePostRequest<TEntity> where TEntity : Entity
	{
		[Required]
		public int Id { get; set; }
	}
}