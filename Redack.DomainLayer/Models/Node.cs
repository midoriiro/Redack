using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Models
{
    [Table("Nodes")]
    public class Node : Entity
    {
        [Required(ErrorMessage = "The name field is required")]
        [MaxLength(30, ErrorMessage = "Type less than 30 characters")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<Thread> Threads { get; set; }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
