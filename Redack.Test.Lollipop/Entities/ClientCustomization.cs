using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;

namespace Redack.Test.Lollipop.Entities
{
    public class ClientCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new ApiKeyCustomization(256));
            fixture.Customize<Client>(e => e.With(p => p.IsBlocked, false));
        }
    }
}
