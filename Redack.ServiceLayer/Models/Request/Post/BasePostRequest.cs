using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request.Post
{
	public abstract class BasePostRequest<TEntity> : BaseModel, IEntityRequest where TEntity : Entity
	{
		public abstract Entity ToEntity(IDbContext context);
		public abstract Task<Entity> ToEntityAsync(IDbContext context);
		public abstract void FromEntity(Entity entity);
	}
}