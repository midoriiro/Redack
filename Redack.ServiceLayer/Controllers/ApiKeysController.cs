using System;
using System.Collections.Generic;
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
	[RoutePrefix("api/apikeys")]	
	public class ApiKeysController : RepositoryApiController<ApiKey>
	{
		public ApiKeysController(IDbContext context) : base(context)
		{
		}

		public override bool IsOwner(int id)
		{
			throw new NotImplementedException();
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("ApiKey.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("ApiKey.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}
		
		public override async Task<IHttpActionResult> Post([FromBody] BasePostRequest<ApiKey> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("ApiKey.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<ApiKey> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorizationFilter]
		[JwtAuthorize("ApiKey.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}