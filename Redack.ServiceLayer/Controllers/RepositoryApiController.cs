using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Redack.ServiceLayer.Controllers
{
    public abstract class RepositoryApiController<TEntity> : BaseApiController where TEntity : Entity
    {
        private readonly IRepository<TEntity> _repository;

        public RepositoryApiController() : base()
        {
            this._repository = new Repository<TEntity>(this.Context);
        }

        // GET: api/Entities
        [HttpGet]
        [Route("")]
        public virtual IEnumerable<TEntity> GetAll()
        {
            return this._repository.GetAll();
        }

        // GET: api/Entities/5
        [HttpGet]
        [Route("{id:int:min(1)}")]
        [ResponseType(typeof(Entity))]
        public virtual async Task<IHttpActionResult> Get(int id)
        {
            var entity = await this._repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            return this.Ok(entity);
        }

        // PUT: api/Entities/5
        [HttpPut]
        [Route("{id:int:min(1)}")]
        [ResponseType(typeof(void))]
        public virtual async Task<IHttpActionResult> Put(int id, TEntity entity)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            if (id != entity.Id)
                return this.BadRequest();

            this._repository.Update(entity);

            try
            {
                await this._repository.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.EntityExists(id))
                    return this.NotFound();

                throw;
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Entities
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Entity))]
        public virtual async Task<IHttpActionResult> Post(TEntity entity)
        {
            if (!this.ModelState.IsValid)
                return BadRequest(ModelState);

            this._repository.Insert(entity);

            try
            {
                await _repository.CommitAsync();
            }
            catch (DbUpdateException)
            {
                if (this.EntityExists(entity.Id))
                    return this.Conflict();

                throw;
            }

            return this.CreatedAtRoute(WebApiConfig.DefaultRouteName, new { id = entity.Id }, entity);
        }

        // DELETE: api/Entities/5
        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [ResponseType(typeof(Entity))]
        public virtual async Task<IHttpActionResult> Delete(int id)
        {
            TEntity entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return this.NotFound();

            this._repository.Delete(entity);
            await _repository.CommitAsync();

            return this.Ok(entity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._repository.Dispose();
            }

            base.Dispose(disposing);
        }

        protected bool EntityExists(int id)
        {
            return this._repository.Query(e => e.Id == id).Count() == 1;
        }
    }
}