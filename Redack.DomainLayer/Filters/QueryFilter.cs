using System;
using System.Linq.Expressions;
using Redack.DomainLayer.Models;

namespace Redack.DomainLayer.Filters
{
    public class QueryFilter<TEntity> where TEntity : Entity
    {
        public Expression<Func<TEntity, bool>> Predicate { get; set; }
    }
}
