using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
    public class CredentialCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new OmitOnRecursionCustomization());
            fixture.Customize(new EmailAddressCustomization<Credential>("Login"));
            fixture.Customize(new CopyPropertyValueToAnother<Credential>(
                "Password", "PasswordConfirm"));
            fixture.Customize(new ApiKeyCustomization(256));
            fixture.Customize(new IgnorePropertiesCustomization(new[] {"User"}));
        }
    }
}
