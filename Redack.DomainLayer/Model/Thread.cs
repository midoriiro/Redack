using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Threads")]
    public class Thread : Entity
    {
        [Required(ErrorMessage = "The title field is required")]
        [MaxLength(50, ErrorMessage = "Type less than 50 characters")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        [Index(Order = 1)]
        public string Title { get; set; }

        [MaxLength(50, ErrorMessage = "Type less than 50 characters")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        [Index(Order = 2)]
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<Message> Messages { get; set; }

        [Required(ErrorMessage = "The node field is required")]
        public virtual Node Node { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode() +
                   this.Title.GetHashCode() +
                   this.Description.GetHashCode() +
                   this.Node.GetHashCode();
        }
    }
}
