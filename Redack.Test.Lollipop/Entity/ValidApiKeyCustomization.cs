using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Thread = System.Threading.Thread;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidApiKeyCustomization : ICustomization
    {
        private readonly int _keySize;

        public ValidApiKeyCustomization(int keySize)
        {
            this._keySize = keySize;
        }

        public void Customize(IFixture fixture)
        {
            // TODO: fix this workaround

            Thread.Sleep(25);

            fixture.Customize<ApiKey>(e => e.With(p => p.Key, ApiKey.GenerateKey(this._keySize)));
        }
    }
}
