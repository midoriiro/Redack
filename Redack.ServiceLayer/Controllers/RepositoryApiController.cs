using System;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Models.Request;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Redack.ServiceLayer.Controllers
{
	public abstract class RepositoryApiController<TEntity> : 
		BaseApiController, 
		IOwnerFilter,
		IRepositoryApiController<TEntity> 
		where TEntity : Entity
	{
		public IRepository<TEntity> Repository { get; }

		protected RepositoryApiController() : base()
		{
			this.Repository = new Repository<TEntity>(this.Context);
		}

		public abstract bool IsOwner(int id);

		// GET: api/Entities
		[HttpGet]
		[Route("")]
		[ResponseType(typeof(ICollection<Entity>))]
		public virtual async Task<IHttpActionResult> GetAll()
		{
			var entities = await this.Repository.GetAllAsync();

			return this.Ok(entities);
		}

		// GET: api/Entities/5
		[HttpGet]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(Entity))]
		public virtual async Task<IHttpActionResult> Get(int id)
		{
			var entity = await this.Repository.GetByIdAsync(id);

			if (entity == null)
				return NotFound();

			return this.Ok(entity);
		}

		// POST: api/Entities
		[HttpPost]
		[Route("")]
		[ResponseType(typeof(Entity))]
		public virtual async Task<IHttpActionResult> Post([FromBody] BaseRequest<TEntity> request)
		{
			if (!this.ModelState.IsValid)
				return this.BadRequest(ModelState);

			TEntity entity = (TEntity)request.ToEntity(this.Context);

			this.Repository.Insert(entity);

			try
			{
			   await this.Repository.CommitAsync();
			}
			catch (DbEntityValidationException)
			{
				return this.BadRequest(ModelState);
			}
			catch (DbUpdateException)
			{
				return this.Conflict();
			}

			var result = this.CreatedAtRoute(
				WebApiConfig.DefaultRouteName, 
				new
				{
					controller = this.GetControllerRouteName(),
					id = entity.Id
				}, 
				entity);

			return result;
		}

		// PUT: api/Entities/5
		[HttpPut]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(void))]
		public virtual async Task<IHttpActionResult> Put(int id, [FromBody] BasePutRequest<TEntity> request)
		{
			if (!this.ModelState.IsValid)
				return this.BadRequest(this.ModelState);

			TEntity entity = (TEntity)request.ToEntity(this.Context);

			if (entity == null)
				return this.NotFound();

			if (id != entity.Id)
				return this.BadRequest();

			this.Repository.Update(entity);

			try
			{
				await this.Repository.CommitAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!this.EntityExists(id))
					return this.NotFound();

				throw;
			}

			return this.StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/Entities/5
		[HttpDelete]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(Entity))]
		public virtual async Task<IHttpActionResult> Delete(int id)
		{
			TEntity entity = await Repository.GetByIdAsync(id);

			if (entity == null)
				return this.NotFound();

			this.Repository.Delete(entity);
			await Repository.CommitAsync();

			return this.Ok(entity);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Repository.Dispose();
			}

			base.Dispose(disposing);
		}

		protected bool EntityExists(int id)
		{
			return this.Repository.All().Any(e => e.Id == id);
		}
	}
}