using System;
using System.Linq;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;
using Redack.ServiceLayer.Models.Request.Post;
using Redack.ServiceLayer.Models.Request.Put;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/groups")]
	[JwtAuthorizationFilter]
	public class GroupsController : RepositoryApiController<Group>
	{
		public GroupsController(IDbContext context) : base(context)
		{
		}

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
		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<Group> request)
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