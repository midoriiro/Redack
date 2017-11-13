using System;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/clients")]
	[JwtAuthorizationFilter]
	public class ClientsController : RepositoryApiController<Client>
	{
		public override bool IsOwner(int id)
		{
			throw new NotImplementedException();
		}

		[JwtAuthorize("Client.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorize("Client.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtAuthorize("Client.Create", "Administrator")]
		public override async Task<IHttpActionResult> Post([FromBody] BaseRequest<Client> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorize("Client.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Client> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorize("Client.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}