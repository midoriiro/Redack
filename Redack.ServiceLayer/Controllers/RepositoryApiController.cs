using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.ServiceLayer.Models.Request.Post;
using Redack.ServiceLayer.Models.Request.Put;
using Redack.ServiceLayer.Models.Request.Uri;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;

namespace Redack.ServiceLayer.Controllers
{
	public abstract class RepositoryApiController<TEntity> : 
		BaseApiController, 
		IOwnerFilter,
		IRepositoryApiController<TEntity> 
		where TEntity : Entity
	{
		public IRepository<TEntity> Repository { get; }
		protected QueryBuilderPolicies Policies { get; private set; }

		protected RepositoryApiController(IDbContext context) : base(context)
		{
			this.Repository = new Repository<TEntity>(this.Context);

			this.Policies = new QueryBuilderPolicies();
			this.Policies.RegisterKeyword("paginate", typeof(PageParameter));
			this.Policies.RegisterKeyword("query", typeof(ExpressionParameter));
			this.Policies.RegisterKeyword("inclose", typeof(ExpressionParameter));
			this.Policies.RegisterKeyword("reshape", typeof(ExpressionParameter));
			this.Policies.RegisterKeyword("order", typeof(ExpressionParameter));
			this.Policies.RegisterKeyword("metadata", typeof(BoolParameter));

			this.Policies.RegisterExpression("query", (q, e) => q.Where(e));
			this.Policies.RegisterExpression("inclose", (q, e) => q.Include(e));
			this.Policies.RegisterExpression("reshape", (q, e) => q.Select(e));
			this.Policies.RegisterExpression("order", (q, e) => q.OrderBy(e));

			this.Policies.ExcludeExpressionKeyword("Credential");
			this.Policies.ExcludeExpressionKeyword("Identities");
			this.Policies.ExcludeExpressionKeyword("Permissions");
			this.Policies.ExcludeExpressionKeyword("ApiKey");
			this.Policies.ExcludeExpressionKeyword("Client");
			this.Policies.ExcludeExpressionKeyword("credential");
			this.Policies.ExcludeExpressionKeyword("identities");
			this.Policies.ExcludeExpressionKeyword("permissions");
			this.Policies.ExcludeExpressionKeyword("apiKey");
			this.Policies.ExcludeExpressionKeyword("client");

			this.Policies.RequiredPagination = true;
		}

		public abstract bool IsOwner(int id);

		// GET: api/Entities
		[HttpGet]
		[Route("")]
		[ResponseType(typeof(void))]
		public virtual async Task<IHttpActionResult> GetAll()
		{
			this.Context.Configuration.ProxyCreationEnabled = false;

			var builder = new QueryBuilder
			{
				Policies = this.Policies
			};

			try
			{
				builder.FromRequest(this.Request);

				var query = (IQueryable)this.Repository.All();
				query = builder.Execute(query);

				List<dynamic> result = await query.ToListAsync();

				return this.Ok(new
				{
					metadata = new object(),
					records = result
				});
			}
			catch(UnauthorizedAccessException)
			{
				return this.Unauthorized();
			}
			catch (QueryBuilderException e)
			{
				this.Configuration
					.Formatters
					.JsonFormatter
					.SerializerSettings
					.PreserveReferencesHandling = PreserveReferencesHandling.None;

				return this.ResponseMessage(
					this.Request.CreateErrorResponse(
						HttpStatusCode.BadRequest,
						$"Query string error : {e.Message}"));
			}
			catch (Exception)
			{
				return this.BadRequest();
			}
		}

		// GET: api/Entities/5
		[HttpGet]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(IEntity))]
		public virtual async Task<IHttpActionResult> Get(int id)
		{
			this.Context.Configuration.ProxyCreationEnabled = false;

			var entity = await this.Repository.GetByIdAsync(id);

			if (entity == null)
				return NotFound();

			return this.Ok(entity);
		}

		// POST: api/Entities
		[HttpPost]
		[Route("")]
		[ResponseType(typeof(IEntity))]
		public virtual async Task<IHttpActionResult> Post([FromBody] BasePostRequest<TEntity> request)
		{
			if (!this.ModelState.IsValid)
				return this.BadRequest(ModelState);

			TEntity entity;

			try
			{
				entity = (TEntity)request.ToEntity(this.Context);
			}
			catch (NotImplementedException)
			{
				entity = (TEntity) await request.ToEntityAsync(this.Context);
			}

			this.Repository.Insert(entity);

			try
			{
			   await this.Repository.CommitAsync();
			}
			catch (DbEntityValidationException)
			{
				this.Validate(entity);
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

			TEntity entity;

			try
			{
				entity = (TEntity)request.ToEntity(this.Context);
			}
			catch (NotImplementedException)
			{
				entity = (TEntity)await request.ToEntityAsync(this.Context);
			}

			if (entity == null)
				return this.NotFound();

			if (id != entity.Id)
				return this.BadRequest();

			this.Repository.Update(entity);

			try
			{
				await this.Repository.CommitAsync();
			}
			catch (DbEntityValidationException)
			{
				this.Validate(entity);
				return this.BadRequest(ModelState);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!this.EntityExists(id))
					return this.NotFound();

				return this.Conflict();
			}

			return this.StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/Entities/5
		[HttpDelete]
		[Route("{id:int:min(1)}")]
		[ResponseType(typeof(IEntity))]
		public virtual async Task<IHttpActionResult> Delete(int id)
		{
			TEntity entity = await Repository.GetByIdAsync(id);

			if (entity == null)
				return this.NotFound();

			this.Repository.Delete(entity);
			await Repository.CommitAsync();

			return this.StatusCode(HttpStatusCode.NoContent);
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