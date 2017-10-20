using System.Collections.Generic;
using Redack.DomainLayer.Filters;

namespace Redack.Test.Lollipop.Data
{
    public class DummyEntity : DomainLayer.Models.Entity
    {
        public string Property1 { get; set; }

        // Navigation properties
        public virtual DummyEntity Property2 { get; set; }

        public override List<QueryFilter<DomainLayer.Models.Entity>> Retrieve()
        {
            throw new System.NotImplementedException();
        }

        public override List<DomainLayer.Models.Entity> Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
