using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
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
        public virtual IList<Message> Messages { get; set; }

        [Required(ErrorMessage = "The node field is required")]
        public virtual Node Node { get; set; }

        public override void Delete()
        {
            for (int i = 0; i < this.Node.Threads.Count; i++)
            {
                var thread = this.Node.Threads.ElementAt(i);

                if (thread.Id == this.Id)
                    this.Node.Threads.RemoveAt(i);
            }

            this.Messages.Clear();
        }
    }
}
