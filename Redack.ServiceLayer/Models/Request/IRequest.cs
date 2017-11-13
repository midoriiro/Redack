using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public interface IRequest
	{
		Entity ToEntity(RedackDbContext context);
		void FromEntity(Entity entity);
	}
}