﻿using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/nodes")]
	public class NodesController : RepositoryApiController<Node>
	{
		public NodesController(IDbContext context) : base(context)
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
		[JwtAuthorize("Node.Create", "Administrator")]
		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<Node> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("Node.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<Node> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("Node.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}