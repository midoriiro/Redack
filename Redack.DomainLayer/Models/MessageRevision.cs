using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Models
{
    [Table("MessageRevisions")]
    public class MessageRevision : Entity
    {
        [Key]
        [Required(ErrorMessage = "The date field is required")]
        public DateTime Date { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The editor field is required")]
        public virtual User Editor { get; set; }
        
        [Required(ErrorMessage = "The message field is required")]
        public virtual Message Message { get; set; }

        public MessageRevision()
        {
            this.Date = DateTime.Now;
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
