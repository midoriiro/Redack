using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly RedackDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(RedackDbContext context = null)
        {
            if (context is null)
                this._context = new RedackDbContext(ConfigurationManager.AppSettings["RedackDbContext"]);
            else
                this._context = context;

            this._entities = this._context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return this._entities.ToList();
        }

        public T GetById(int id)
        {
            return this._entities.Find(id);
        }

        public void Insert(T entity)
        {
            this._entities.Add(entity);
        }

        public void Update(T entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
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
            foreach(DbEntityEntry entry in this._context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;   
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            }
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
