using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/nodes")]
	[JwtAuthorizationFilter]
	public class NodesController : RepositoryApiController<Node>
	{
		public override bool IsOwner(int id)
		{
			throw new System.NotImplementedException();
		}

		[JwtAuthorize("Node.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorize("Node.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtAuthorize("Node.Create", "Administrator")]
		public override async Task<IHttpActionResult> Post([FromBody] BaseRequest<Node> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorize("Node.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Node> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorize("Node.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}