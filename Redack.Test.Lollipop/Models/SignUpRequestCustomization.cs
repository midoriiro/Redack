using Ploeh.AutoFixture;
using Redack.ServiceLayer.Models.Request;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Models
{
    public class SignUpRequestCustomization : SignInRequestCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new CopyPropertyValueToAnother<SignUpRequest>(
                "Password", "PasswordConfirm"));
        }
    }
}
