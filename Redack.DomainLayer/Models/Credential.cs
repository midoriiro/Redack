using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Redack.DomainLayer.Models
{
    [Table("Credentials")]
    public class Credential : Entity
    {
        [Required(ErrorMessage = "The login field is required")]
        [MaxLength(50, ErrorMessage = "Type less than 50 characters")]
        [Index(IsUnique = true)]
        [EmailAddress(ErrorMessage = "The login field is not a valid email address")]
        public string Login { get; set; }

        [Required(ErrorMessage = "The password field is required")]
        [MinLength(8, ErrorMessage = "Type at least 8 characters to ensure password security")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The password confirmation field is required")]
        [Compare("Password", ErrorMessage = "Password confirmation does not match")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "The salt field is required")]
        public string Salt { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The api key field is required")]
        public virtual ApiKey ApiKey { get; set; }

        [InverseProperty("Credential")]
        public virtual User User { get; set; }

        public static string ToHash(string data, byte[] salt)
        {
            byte[] bytes = KeyDerivation.Pbkdf2(
                password: data,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 512 / 8);

            return Convert.ToBase64String(bytes);
        }

        public void ToHash()
        {
            byte[] salt = new byte[256 / 8];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetBytes(salt);

            this.Salt = Convert.ToBase64String(salt);

            this.Password = ToHash(this.Password, salt);
            this.PasswordConfirm = ToHash(this.PasswordConfirm, salt);
        }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            var q = query as IQueryable<Credential>;

            return (q ?? throw new InvalidOperationException()).Where(e => e.User.IsEnabled);
        }

        public override List<Entity> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
