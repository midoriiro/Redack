using System;
using System.Collections.Generic;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Commit();
        void Rollback();
    }
}
