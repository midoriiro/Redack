using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;

namespace Redack.Test.Lollipop.Entity
{
    class ValidApiKeyCustomization : ICustomization
    {
        private readonly int _keySize;

        public ValidApiKeyCustomization(int keySize)
        {
            this._keySize = keySize;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize<ApiKey>(e => e.With(p => p.Key, ApiKey.GenerateKey(this._keySize)));
        }
    }
}
