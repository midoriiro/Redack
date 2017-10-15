using Ploeh.AutoFixture;
using Redack.ServiceLayer.Models;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Entity;

namespace Redack.Test.Lollipop.Model
{
    public class ValidSignInRequestCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            fixture.Customize(new EmailAddressCustomization<SignInRequest>("Login"));
            fixture.Customize(new ValidClientCustomization());
        }
    }
}
