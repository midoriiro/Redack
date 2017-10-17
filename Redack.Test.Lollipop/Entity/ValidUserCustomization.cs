using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidUserCustomization : BaseValidEntityCustomization, ICustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new OmitOnRecursionCustomization(2));
            fixture.Customize(new ValidCredentialCustomization());
            fixture.Customize(new IgnorePropertiesCustomization(new string[]
            {
                "Messages",
                "Groups",
                "Permissions",
                "Identities"
            }));
            fixture.Customize(new StringMaxLengthCustomization<User>("Alias", 15));
        }
    }
}
