using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/messagerevisions")]
	public class MessageRevisionsController : RepositoryApiController<MessageRevision>
	{
		public MessageRevisionsController(IDbContext context) : base(context)
		{
		}

		public override bool IsOwner(int id)
		{
			throw new System.NotImplementedException();
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
		[JwtAuthorize("MessageRevision.Create", "Moderator")]
		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<MessageRevision> request)
		{
			return await base.Post(request);
		}

		[JwtDisableAction]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<MessageRevision> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("MessageRevision.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}