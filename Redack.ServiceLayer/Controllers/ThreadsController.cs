using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/threads")]
	[JwtAuthorizationFilter]
	public class ThreadsController : RepositoryApiController<Thread>
	{
		public override bool IsOwner(int id)
		{
			throw new System.NotImplementedException();
		}

		[JwtAuthorize("Thread.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorize("Thread.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtAuthorize("Thread.Create", "Administrator")]
		public override async Task<IHttpActionResult> Post([FromBody] BaseRequest<Thread> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorize("Thread.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Thread> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorize("Thread.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}