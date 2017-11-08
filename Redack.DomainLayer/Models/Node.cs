using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
{
    [Table("Nodes")]
    public class Node : Entity
    {
        [Required(ErrorMessage = "The name field is required")]
        [MaxLength(30, ErrorMessage = "Type less than 30 characters")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        // Navigation properties
        public virtual IList<Thread> Threads { get; set; }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            throw new NotImplementedException();
        }

        public override List<Entity> Delete()
        {
            List<Entity> result = new List<Entity>();
            result.AddRange(this.Threads);

            this.Threads.Clear();

            return result;
        }
    }
}
