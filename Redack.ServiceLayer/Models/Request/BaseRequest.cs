using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public abstract class BaseRequest<TEntity> : BaseModel, IRequest where TEntity : Entity
	{
		public abstract Entity ToEntity(RedackDbContext context);
		public abstract void FromEntity(Entity entity);
	}
}