using Ploeh.AutoFixture;
using Redack.BridgeLayer.Messages.Request.Post;
using Redack.Test.Lollipop.Customizations;
using Redack.Test.Lollipop.Entities;

namespace Redack.Test.Lollipop.Models
{
	public class ForgotPasswordRequestCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            fixture.Customize(new EmailAddressCustomization<ForgotPasswordRequest>("Login"));
            fixture.Customize(new CopyPropertyValueToAnother<ForgotPasswordRequest>(
                "NewPassword", "NewPasswordConfirm"));
            fixture.Customize(new ClientCustomization());
        }
    }
}
