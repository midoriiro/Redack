using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
	public class GroupCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new StringMaxLengthCustomization<Group>("Name", 15));
            fixture.Customize(new IgnorePropertiesCustomization(new[]
            {
                "Users",
                "Permissions"
            }));
        }
    }
}
