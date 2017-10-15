using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidUserCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new OmitOnRecursionCustomization(2));
            fixture.Customize(new ValidCredentialCustomization());
            fixture.Customize(new IgnorePropertiesCustomization(new string[]
            {
                "Messages",
                "Group",
                "Permissions",
                "Identities"
            }));
            fixture.Customize(new StringMaxLengthCustomization<User>("Alias", 15));
        }
    }
}
