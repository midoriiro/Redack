using System;
using System.ComponentModel.DataAnnotations;

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

        public Client()
        {
            this.IsBlocked = false;
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
