using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Models
{
    [Table("MessageHistories")]
    public class MessageHistory : Entity
    {
        [Required(ErrorMessage = "The date field is required")]
        public DateTime Date { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The editor field is required")]
        public virtual User Editor { get; set; }

        public MessageHistory()
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
