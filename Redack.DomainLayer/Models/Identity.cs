﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
{
    [Table("Identities")]
    public class Identity : Entity
    {
        [Required(ErrorMessage = "The access token field is required")]
        public string Access { get; set; }

        [Required(ErrorMessage = "The refresh token field is required")]
        public string Refresh { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The user field is required")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "The client field is required")]
        public virtual Client Client { get; set; }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            var q = query as IQueryable<Identity>;

            return (q ?? throw new InvalidOperationException()).Where(e => e.User.IsEnabled);
        }

        public override List<Entity> Delete()
        {
            throw new NotImplementedException();
        }
    }
}