using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Threading.Tasks;

namespace Redack.ServiceLayer.Models.Request
{
	public interface IEntityRequest
	{
		Entity ToEntity(IDbContext context);
		Task<Entity> ToEntityAsync(IDbContext context);
		void FromEntity(Entity entity);
	}
}