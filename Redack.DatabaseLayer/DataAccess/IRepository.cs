using System;
using System.Collections.Generic;
using System.Data.Entity;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        bool Exists(TEntity entity);
        List<TEntity> GetAll();
        TEntity GetById(int id);
        TEntity GetOrInsert(TEntity entity);
        void InsertOrUpdate(TEntity entity);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Commit();
        void Rollback();
    }
}
