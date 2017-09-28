﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Redack.DomainLayer.Exception;

namespace Redack.DomainLayer.Model
{
    [Table("Credentials")]
    public class Credential : Entity, IValidatableObject
    {
        [Required(ErrorMessage = "The login field is required")]
        [MaxLength(50, ErrorMessage = "Type less than 50 characters")]
        [Index(IsUnique = true)]
        [EmailAddress]
        public string Login { get; set; }

        [Required(ErrorMessage = "The password field is required")]
        [MinLength(8, ErrorMessage = "Type at least 8 characters to ensure password security")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The password confirmation field is required")]
        [Compare("Password", ErrorMessage = "Password confirmation does not match")]
        public string PasswordConfirm { get; set; }

        // Navigation properties
        public virtual ICollection<Credential> OldCredentials { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Password != this.PasswordConfirm)
            {
                yield return new ValidationResult(new CredentialPasswordConfirmException().Message);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() +
                this.Login.GetHashCode() +
                this.Password.GetHashCode() +
                this.PasswordConfirm.GetHashCode() +
                this.GetHashCode();
        }
    }
}
