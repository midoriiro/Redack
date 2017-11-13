using System;
using System.Linq;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/groups")]
	[JwtAuthorizationFilter]
	public class GroupsController : RepositoryApiController<Group>
	{
		public override bool IsOwner(int id)
		{
			throw new NotImplementedException();
		}

		[JwtAuthorize("Group.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorize("Group.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtAuthorize("Group.Create", "Administrator")]
		public override async Task<IHttpActionResult> Post([FromBody] BaseRequest<Group> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorize("Group.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Group> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorize("Group.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}