using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
    public class UserCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new OmitOnRecursionCustomization(2));
            fixture.Customize(new CredentialCustomization());
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
