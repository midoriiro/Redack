using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly RedackDbContext<T> context;

        public Repository()
        {
            this.context = new RedackDbContext<T>();
        }

        public IEnumerable<T> All()
        {
            return this.context.Entities.ToList();
        }

        public T Get(int id)
        {
            return this.context.Entities.Find(id);
        }

        public void Insert(T entity)
        {
            this.context.Entities.Add(entity);
        }

        public void Update(T entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            this.context.Entities.Attach(entity);
            this.context.Entities.Remove(entity);
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Rollback()
        {
            foreach(DbEntityEntry entry in this.context.ChangeTracker.Entries())
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
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    this.context.Dispose();

                disposed = true;
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
