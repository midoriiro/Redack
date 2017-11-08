using Redack.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Redack.DatabaseLayer.DataAccess
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IEntity
    {
        IQueryable<TEntity> All();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);
        TEntity GetOrInsert(TEntity entity);
        void InsertOrUpdate(TEntity entity);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Commit();
        Task<int> CommitAsync();
        void Rollback();
        bool Exists(TEntity entity);
        Task<bool> ExistsAsync(TEntity entity);
    }
}
