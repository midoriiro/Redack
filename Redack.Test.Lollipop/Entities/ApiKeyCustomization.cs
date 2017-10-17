using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;

namespace Redack.Test.Lollipop.Entities
{
    public class ApiKeyCustomization : BaseEntityCustomization
    {
        private readonly int _keySize;

        public ApiKeyCustomization(int keySize)
        {
            this._keySize = keySize;
        }

        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize<ApiKey>(e => e.With(p => p.Key, ApiKey.GenerateKey(this._keySize)));
        }
    }
}
