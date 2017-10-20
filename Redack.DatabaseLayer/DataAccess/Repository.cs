using Redack.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Redack.DatabaseLayer.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IDbContext _context;
        private readonly DbSet<TEntity> _entities;

        private bool _disposable { get; set; } = true;
        public bool Disposed { get; private set; } = false;

        public Repository(IDbContext context = null)
        {
            if (context is null)
                this._context = new RedackDbContext();
            else
                this._context = context;

            this._entities = this._context.Set<TEntity>();
        }

        public Repository(IDbContext context, bool disposable)
        {
            this._disposable = disposable;
            this._context = context;

            this._entities = this._context.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return this._entities;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return this._entities.Where(predicate);
        }

        public List<TEntity> GetAll()
        {
            return this._entities.ToList<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await this._entities.ToListAsync();
        }

        public TEntity GetById(params object[] ids)
        {
            return this._entities.Find(ids);
        }

        public async Task<TEntity> GetByIdAsync(params object[] ids)
        {
            return await this._entities.FindAsync(ids);
        }

        public TEntity GetOrInsert(TEntity entity)
        {
            TEntity obj = this.GetById(entity.Id);

            if (obj != null) return obj;

            this.Insert(entity);

            return entity;
        }

        public void InsertOrUpdate(TEntity entity)
        {
            if (!this.Exists(entity))
                this.Insert(entity);
            else
                this.Update(entity);
        }

        public void Insert(TEntity entity)
        {
            this._entities.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (this._context.Entry(entity).State == EntityState.Detached)
                this._entities.Attach(entity);

            this._context.SetEntityState(entity, EntityState.Modified);
        }

        public void Delete(TEntity entity)
        {
            if(this._context.Entry(entity).State == EntityState.Detached)
                this._entities.Attach(entity);
            try
            {
                List<Entity> entities = entity.Delete();

                if (entities != null)
                {
                    foreach (var e in entities)
                    {
                        this._context.Entry(e).State = EntityState.Unchanged;
                        this._context.Entry(e).State = EntityState.Deleted;
                    }
                }
            }
            catch (NotImplementedException) { }

            this._entities.Remove(entity);
        }

        public void Commit()
        {
            this._context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await this._context.SaveChangesAsync();
        }

        public void Rollback()
        {
            this._context.Rollback();
        }

        public bool Exists(TEntity entity)
        {
            return this._entities.Count(e => e.Id == entity.Id) == 1;
        }

        public async Task<bool> ExistsAsync(TEntity entity)
        {
            return await this._entities.CountAsync(e => e.Id == entity.Id) == 1;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                    this._context.Dispose();

                this.Disposed = true;
            }
        }

        public void Dispose()
        {
            if (this._disposable)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
