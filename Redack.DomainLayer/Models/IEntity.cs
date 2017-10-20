using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Redack.DomainLayer.Filters;

namespace Redack.DomainLayer.Models
{
    public interface IEntity
    {
        List<QueryFilter<Entity>> Retrieve();
        List<Entity> Delete();
    }
}