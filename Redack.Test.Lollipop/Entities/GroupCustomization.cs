using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;
using Redack.Test.Lollipop.Entities;

namespace Redack.Test.Lollipop.Entity
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
