using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.Test.Data
{
    public class DummyEntity : Entity
    {
        public string Property1 { get; set; }

        // Navigation properties
        public DummyEntity Property2 { get; set; }
    }
}
