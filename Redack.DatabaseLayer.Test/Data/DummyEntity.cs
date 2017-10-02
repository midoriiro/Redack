using System.ComponentModel.DataAnnotations;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.Test.Data
{
    public class DummyEntity : DomainLayer.Model.Entity
    {
        public string Property1 { get; set; }

        // Navigation properties
        public virtual DummyEntity Property2 { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode() +
                   this.Property1.GetHashCode() +
                   this.Property2.GetHashCode();
        }
    }
}
