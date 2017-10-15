using Ploeh.AutoFixture;
using Redack.ServiceLayer.Models;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Entity;

namespace Redack.Test.Lollipop.Model
{
    public class ValidSignUpRequestCustomization : ValidSignInRequestCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new CopyPropertyValueToAnother<SignUpRequest>(
                "Password", "PasswordConfirm"));
        }
    }
}
