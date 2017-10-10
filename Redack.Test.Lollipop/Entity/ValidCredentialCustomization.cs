using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidCredentialCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new OmitOnRecursionCustomization(1));
            fixture.Customize(new EmailAddressCustomization<Credential>("Login"));
            fixture.Customize(new CopyPropertyValueToAnother<Credential>(
                "Password", "PasswordConfirm"));
            fixture.Customize(new ValidApiKeyCustomization(256));
        }
    }
}
