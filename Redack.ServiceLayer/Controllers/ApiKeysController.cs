﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redack.DomainLayer.Models;
using System.Web.Http;
using Redack.ServiceLayer.Filters;
using Redack.ServiceLayer.Models.Request;

namespace Redack.ServiceLayer.Controllers
{
	[RoutePrefix("api/apikeys")]
	[JwtAuthorizationFilter]
	public class ApiKeysController : RepositoryApiController<ApiKey>
	{
		public override bool IsOwner(int id)
		{
			throw new NotImplementedException();
		}

		[JwtAuthorize("ApiKey.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> GetAll()
		{
			return await base.GetAll();
		}

		[JwtAuthorize("ApiKey.Retrieve", "Administrator")]
		public override async Task<IHttpActionResult> Get(int id)
		{
			return await base.Get(id);
		}

		[JwtDisableAction]
		public override async Task<IHttpActionResult> Post([FromBody] BaseRequest<ApiKey> request)
		{
			return await base.Post(request);
		}

		[JwtAuthorize("ApiKey.Update", "Administrator")]
		public override async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<ApiKey> request)
		{
			return await base.Put(id, request);
		}

		[JwtAuthorize("ApiKey.Delete", "Administrator")]
		public override async Task<IHttpActionResult> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}