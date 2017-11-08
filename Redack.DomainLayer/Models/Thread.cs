using System;
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
        public virtual IList<Message> Messages { get; set; } = new List<Message>();

        [Required(ErrorMessage = "The node field is required")]
        public virtual Node Node { get; set; }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            throw new NotImplementedException();
        }

        public override List<Entity> Delete()
        {
            for (int i = 0; i < this.Node.Threads.Count; i++)
            {
                var thread = this.Node.Threads.ElementAt(i);

                if (thread.Id == this.Id)
                    this.Node.Threads.RemoveAt(i);
            }

            foreach (var message in this.Messages)
            {
                for (int i = 0; i < message.Author.Messages.Count; i++)
                {
                    var m = message.Author.Messages.ElementAt(i);

                    if (m.Id == message.Id)
                        message.Author.Messages.RemoveAt(i);
                }
            }

            this.Node = null;

            List<Entity> result = new List<Entity>();
            result.AddRange(this.Messages);

            

            this.Messages.Clear();

            return result;
        }
    }
}
