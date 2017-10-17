using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IDbContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(IDbContext context = null)
        {
            if (context is null)
                this._context = new RedackDbContext();
            else
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

        public TEntity GetById(int id)
        {
            return this._entities.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await this._entities.FindAsync(id);
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
            try
            {
                entity.Update();
            }
            catch (NotImplementedException){}

            this._entities.Attach(entity);
            this._context.SetEntityState(entity, EntityState.Modified);
        }

        public void Delete(TEntity entity)
        {
            try
            {
                entity.Delete();
            }
            catch (NotImplementedException){}

            this._entities.Attach(entity);
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

        #region IDisposable Support
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    this._context.Dispose();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
