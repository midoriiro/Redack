using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redack.DomainLayer.Model
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
