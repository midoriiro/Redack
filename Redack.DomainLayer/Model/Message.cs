using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Messages")]
    public class Message : Entity
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required(ErrorMessage = "The text field is required")]
        [MinLength(15, ErrorMessage = "Type at least 15 characters")]
        public string Text { get; set; }

        // Navigation properties

        [Required(ErrorMessage = "The thread field is required")]
        public virtual Thread Thread { get; set; }

        [Index]
        public virtual User CreatedBy { get; set; }

        public virtual User UpdatedBy { get; set; }

        public Message()
        {
            this.DateCreated = DateTime.Now;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() +
                   this.DateCreated.GetHashCode() +
                   this.DateUpdated.GetHashCode() +
                   this.Text.GetHashCode() +
                   this.CreatedBy.GetHashCode() +
                   this.UpdatedBy.GetHashCode();
        }
    }
}
