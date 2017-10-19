using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
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

        public virtual ICollection<MessageRevision> Revisions { get; set; }

        public Message()
        {
            this.Date = DateTime.Now;
        }

        public override void Delete()
        {
            /*for (int i = 0; i < this.Author.Messages.Count; i++)
            {
                var message = this.Author.Messages.ElementAt(i);

                if (message.Id == this.Id)
                    this.Author.Messages.RemoveAt(i);
            }*/
        }
    }
}
