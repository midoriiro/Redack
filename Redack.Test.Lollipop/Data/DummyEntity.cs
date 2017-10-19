namespace Redack.Test.Lollipop.Data
{
    public class DummyEntity : DomainLayer.Models.Entity
    {
        public string Property1 { get; set; }

        // Navigation properties
        public virtual DummyEntity Property2 { get; set; }

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
