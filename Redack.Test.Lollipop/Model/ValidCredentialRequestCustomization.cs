using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Entity;

namespace Redack.Test.Lollipop.Model
{
    public class ValidCredentialRequestCustomization<T> : ICustomization where T : class
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new OmitOnRecursionCustomization());
            fixture.Customize(new EmailAddressCustomization<T>("Login"));
            fixture.Customize(new CopyPropertyValueToAnother<T>(
                "Password", "PasswordConfirm"));
            fixture.Customize(new ValidClientCustomization());
        }
    }
}
