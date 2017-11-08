using System.Collections.Generic;
using System.Linq;

namespace Redack.DomainLayer.Models
{
    public interface IEntity
    {
        bool CanBeDeleted { get; }

        IQueryable<Entity> Filter(IQueryable<Entity> query);
        List<Entity> Delete();
    }
}