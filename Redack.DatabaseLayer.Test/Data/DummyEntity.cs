using System.ComponentModel.DataAnnotations;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.Test.Data
{
    public class DummyEntity : Entity
    {
        [Required]
        [MaxLength(10)]
        public string Property1 { get; set; }

        // Navigation properties
        public virtual DummyEntity Property2 { get; set; }
    }
}
