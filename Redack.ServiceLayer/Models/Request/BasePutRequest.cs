using System.ComponentModel.DataAnnotations;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public abstract class BasePutRequest<TEntity> : BaseRequest<TEntity> where TEntity : Entity
	{
		[Required]
		public int Id { get; set; }
	}
}