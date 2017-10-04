namespace Redack.Test.Lollipop.Data
{
    public class DummyEntity : DomainLayer.Model.Entity
    {
        public string Property1 { get; set; }

        // Navigation properties
        public virtual DummyEntity Property2 { get; set; }
    }
}
