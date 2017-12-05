﻿using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.BridgeLayer.Messages.Request.Put;

namespace Redack.ServiceLayer.Controllers
{
	public interface IRepositoryApiController<TEntity> where TEntity : Entity
	{
		IRepository<TEntity> Repository { get; }

		[HttpGet]
		[Route("")]
		[ResponseType(typeof(ICollection<Entity>))]
		Task<IHttpActionResult> GetAll();

		[HttpGet]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(Entity))]
		Task<IHttpActionResult> Get(int id);

		[HttpPut]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(void))]
		Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<TEntity> request);

		[HttpPost]
		[Route("")]
		[ResponseType(typeof(Entity))]
		Task<IHttpActionResult> Post([FromBody] BasePostRequest<TEntity> request);

		[HttpDelete]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(Entity))]
		Task<IHttpActionResult> Delete(int id);
	}
}