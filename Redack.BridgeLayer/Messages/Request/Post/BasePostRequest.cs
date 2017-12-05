using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Threading.Tasks;

namespace Redack.BridgeLayer.Messages.Request.Post
{
	public abstract class BasePostRequest<TEntity> : BaseModel, IEntityRequest where TEntity : Entity
	{
		public abstract Entity ToEntity(IDbContext context);
		public abstract Task<Entity> ToEntityAsync(IDbContext context);
		public abstract void FromEntity(Entity entity);
	}
}