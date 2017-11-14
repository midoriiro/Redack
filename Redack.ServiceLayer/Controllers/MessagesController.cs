using System.Linq;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models;
using Redack.ServiceLayer.Models.Request;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/messages")]
	public class MessagesController : RepositoryApiController<Message>
	{
		public override bool IsOwner(int id)
		{
			var user = this.GetIdentity().User;

			return this.Repository.All()
				.Where(e => e.Id == id)
				.Any(e => e.Author.Id == user.Id);
		}

		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtAuthorizationFilter]
		public override async Task<IHttpActionResult> Post([FromBody] BaseRequest<Message> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorizeOwner("id")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Message> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorizeOwner("id")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}