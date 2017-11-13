using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/permissions")]
	[JwtAuthorizationFilter]
	public class PermissionsController : RepositoryApiController<Permission>
	{
		public override bool IsOwner(int id)
		{
			throw new System.NotImplementedException();
		}
	}
}