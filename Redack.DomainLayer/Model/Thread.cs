using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Threads")]
    public class Thread : Entity
    {
        [Required(ErrorMessage = "The title field is required")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        public string Title { get; set; }

        [MinLength(5)]
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<Message> Messages { get; set; }
        public virtual Node Node { get; set; }
	}
}
