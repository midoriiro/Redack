using Ploeh.AutoFixture;
using Redack.ServiceLayer.Models;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Entity;

namespace Redack.Test.Lollipop.Model
{
    public class ValidForgotPasswordRequestCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            fixture.Customize(new EmailAddressCustomization<ForgotPasswordRequest>("Login"));
            fixture.Customize(new CopyPropertyValueToAnother<ForgotPasswordRequest>(
                "NewPassword", "NewPasswordConfirm"));
            fixture.Customize(new ValidClientCustomization());
        }
    }
}
