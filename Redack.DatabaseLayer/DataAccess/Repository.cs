using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IDbContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(IDbContext context = null)
        {
            if(context is null)
                this._context = new RedackDbContext(ConfigurationManager.AppSettings["RedackDbContext"]);
            else
                this._context = context;

            this._entities = this._context.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return this._entities.ToList<TEntity>();
        }

        public TEntity GetById(int id)
        {
            return this._entities.Find(id);
        }

        public void Insert(TEntity entity)
        {
            this._entities.Add(entity);
        }

        public void Update(TEntity entity)
        {
            this._context.SetEntityState(entity, EntityState.Modified);
        }

        public void Delete(TEntity entity)
        {
            this._entities.Attach(entity);
            this._entities.Remove(entity);
        }

        public void Commit()
        {
            this._context.SaveChanges();
        }

        public void Rollback()
        {
            this._context.Rollback();
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
