using Ploeh.AutoFixture;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop.Customization;
using Thread = System.Threading.Thread;

namespace Redack.Test.Lollipop.Entity
{
    public class ValidGroupCustomization : BaseValidEntityCustomization, ICustomization
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
