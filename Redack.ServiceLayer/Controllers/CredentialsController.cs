using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/credentials")]
	[JwtAuthorizationFilter]
	public class CredentialsController : RepositoryApiController<Credential>
	{
		public CredentialsController(IDbContext context) : base(context)
		{
		}

		public override bool IsOwner(int id)
		{
			return this.GetIdentity().User.Credential.Id == id;
		}

		[JwtAuthorize("Credential.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorizeOwner("id")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtDisableAction]
		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<Credential> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorizeOwner("id")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Credential> request)
		{
			return await base.Put(id, request);
		}

		[JwtDisableAction]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}