using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Redack.DomainLayer.Models
{
    public class Client : Entity
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Type less than 50 characters")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        public string Name { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        // Navigation properties
        [Required]
        public virtual ApiKey ApiKey { get; set; }

        public virtual ICollection<Identity> Identities { get; set; }

        public Client()
        {
            this.IsBlocked = false;
        }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            throw new NotImplementedException();
        }

        public override List<Entity> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
