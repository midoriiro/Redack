using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Nodes")]
    public class Node : Entity
    {
        [Required(ErrorMessage = "The name field is required")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<Thread> Threads { get; set; }
	}
}
