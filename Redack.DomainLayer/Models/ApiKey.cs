using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using LinqKit;

namespace Redack.DomainLayer.Models
{
    [Table("ApiKeys")]
    public class ApiKey : Entity
    {
        [Required(ErrorMessage = "The api key field is required")]
        [MaxLength(255, ErrorMessage = "Type less than 255 characters")]
        [Index(IsUnique = true)]
        public string Key { get; set; }

        // Navigation properties
        [InverseProperty("ApiKey")]
        public Credential Credential { get; set; }

        [InverseProperty("ApiKey")]
        public Client Client { get; set; }

        public static string GenerateKey(int size) => Convert.ToBase64String(new AesCryptoServiceProvider { KeySize = size }.Key);

        public static byte[] ToBytes(string key) => Convert.FromBase64String(key);

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            var predicate = PredicateBuilder.New<ApiKey>();
            predicate.Or(e => e.Credential != null && e.Credential.User.IsEnabled);
            predicate.Or(e => e.Client != null && !e.Client.IsBlocked);

            var q = query as IQueryable<ApiKey>;

            return (q ?? throw new InvalidOperationException()).AsExpandable().Where(predicate);
        }

        public override List<Entity> Delete()
        {
            throw new NotImplementedException();
        }
    }
}