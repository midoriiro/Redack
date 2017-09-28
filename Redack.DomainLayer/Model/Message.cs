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
        public virtual Thread Thread { get; set; }

        [Index]
        public virtual User CreatedBy { get; set; }

        public virtual User UpdatedBy { get; set; }

        public Message()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}
