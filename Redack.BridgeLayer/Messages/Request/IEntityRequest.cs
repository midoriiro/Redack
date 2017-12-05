using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request
{
	public interface IEntityRequest
	{
		Entity ToEntity(IDbContext context);
		Task<Entity> ToEntityAsync(IDbContext context);
		void FromEntity(Entity entity);
	}
}