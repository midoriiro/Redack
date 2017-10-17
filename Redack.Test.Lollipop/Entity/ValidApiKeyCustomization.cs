using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Thread = System.Threading.Thread;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidApiKeyCustomization : BaseValidEntityCustomization, ICustomization
    {
        private readonly int _keySize;

        public ValidApiKeyCustomization(int keySize)
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
