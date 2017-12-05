using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/clients")]	
	public class ClientsController : RepositoryApiController<Client>
	{
		public ClientsController(IDbContext context) : base(context)
		{
		}

		public override bool IsOwner(int id)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		[Route("signin")]
		[ResponseType(typeof(Client))]
		public async Task<IHttpActionResult> SignIn([FromBody] ClientSignInRequest request)
		{
			if (!this.ModelState.IsValid)
				return this.BadRequest(this.ModelState);

			Client client;

			try
			{
				client = (Client)request.ToEntity(this.Context);
			}
			catch (NotImplementedException)
			{
				client = (Client) await request.ToEntityAsync(this.Context);
			}

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