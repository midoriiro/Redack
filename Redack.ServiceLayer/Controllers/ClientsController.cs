using System;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;
using System.Web.Http.Description;
using Redack.ServiceLayer.Models.Request.Post;
using Redack.ServiceLayer.Models.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/clients")]	
	public class ClientsController : RepositoryApiController<Client>
	{
		public override bool IsOwner(int id)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		[Route("signin")]
		[ResponseType(typeof(Client))]
		public virtual IHttpActionResult SignIn([FromBody] ClientSignInRequest request)
		{
			if (!this.ModelState.IsValid)
				return this.BadRequest(this.ModelState);

			var client = (Client)request.ToEntity(this.Context);

			if (client == null || client.IsBlocked)
				return this.Unauthorized();

			return this.Ok(client);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("Client.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("Client.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<Client> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("Client.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Client> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("Client.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}