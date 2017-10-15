using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Jose;
using Newtonsoft.Json;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;

namespace Redack.ServiceLayer.Security
{
    public class JwtTokenizer
    {
        public static string Encode(string key, string issuer, double expiration)
        {
            var payload = new Dictionary<string, object>()
            {
                { "iss", issuer },
                { "iat", DateTime.UtcNow },
                { "exp", DateTime.UtcNow.AddMinutes(expiration)}
            };

            return JWT.Encode(
                payload,
                Convert.FromBase64String(key), 
                JweAlgorithm.A256GCMKW, 
                JweEncryption.A256CBC_HS512);
        }

        public static Dictionary<string, object> Decode(string key, string token)
        {
            string json = JWT.Decode(
                token,
                Convert.FromBase64String(key), 
                JweAlgorithm.A256GCMKW, 
                JweEncryption.A256CBC_HS512);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public static bool IsValid(string key, string issuer, string token)
        {
            var untokenized = Decode(key, token);

            DateTime exp = (DateTime)untokenized["exp"];
            string iss = (string)untokenized["iss"];

            return DateTime.UtcNow <= exp && iss == issuer;
        }
    }
}