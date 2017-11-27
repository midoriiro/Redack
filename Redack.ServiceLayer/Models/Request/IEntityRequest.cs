using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public interface IEntityRequest
	{
		Entity ToEntity(RedackDbContext context);
		void FromEntity(Entity entity);
	}
}