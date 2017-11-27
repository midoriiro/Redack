using System;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using System.Web.Http.Results;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Models.Request.Post;
using Redack.ServiceLayer.Models.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/credentials")]
	[JwtAuthorizationFilter]
	public class CredentialsController : RepositoryApiController<Credential>
	{
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