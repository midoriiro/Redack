using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Redack.DomainLayer.Filters;

namespace Redack.DomainLayer.Models
{
    [Table("ApiKeys")]
    public class ApiKey : Entity
    {
        [Required(ErrorMessage = "The api key field is required")]
        [MaxLength(255, ErrorMessage = "Type less than 255 characters")]
        [Index(IsUnique = true)]
        public string Key { get; set; }

        public static string GenerateKey(int size) => Convert.ToBase64String(new AesCryptoServiceProvider { KeySize = size }.Key);

        public static byte[] ToBytes(string key) => Convert.FromBase64String(key);

        public override List<QueryFilter<Entity>> Retrieve()
        {
            throw new NotImplementedException();
        }

        public override List<Entity> Delete()
        {
            throw new NotImplementedException();
        }
    }
}