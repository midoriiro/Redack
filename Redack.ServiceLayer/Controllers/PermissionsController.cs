using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Models.Request.Post;
using Redack.ServiceLayer.Models.Request.Put;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/permissions")]
	[JwtAuthorizationFilter]
	public class PermissionsController : RepositoryApiController<Permission>
	{
		public PermissionsController(IDbContext context) : base(context)
		{
		}

		public override bool IsOwner(int id)
		{
			throw new System.NotImplementedException();
		}

		[JwtAuthorize("Permission.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorize("Permission.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtAuthorize("Permission.Create", "Administrator")]
		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<Permission> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorize("Permission.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Permission> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorize("Permission.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}