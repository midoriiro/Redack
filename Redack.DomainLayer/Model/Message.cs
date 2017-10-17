using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Redack.DomainLayer.Exception;

namespace Redack.DomainLayer.Model
{
    [Table("Messages")]
    public class Message : Entity
    {
        [Required(ErrorMessage = "The date field is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "The text field is required")]
        [MinLength(15, ErrorMessage = "Type at least 15 characters")]
        public string Text { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The thread field is required")]
        public virtual Thread Thread { get; set; }
        
        [Required(ErrorMessage = "The author field is required")]
        [Index]
        public virtual User Author { get; set; }

        public virtual ICollection<MessageHistory> RevisionHistory { get; set; }

        public Message()
        {
            this.Date = DateTime.Now;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
