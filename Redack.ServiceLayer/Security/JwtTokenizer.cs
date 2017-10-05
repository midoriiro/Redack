using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jose;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;

namespace Redack.ServiceLayer.Security
{
    public class JwtTokenizer
    {
        public string Encode(Identity identity, double expiration)
        {
            var payload = new Dictionary<string, object>()
            {
                { "iss", "redack" },
                { "iat", DateTime.UtcNow },
                { "exp", DateTime.UtcNow.AddMinutes(expiration)}
            };

            return JWT.Encode(
                payload, ApiKey.ToBytes(identity.ApiKey.Key), JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512);
        }

        public string Decode(Identity identity)
        {
            return JWT.Decode(identity.Token, identity.ApiKey.Key);
        }
    }
}