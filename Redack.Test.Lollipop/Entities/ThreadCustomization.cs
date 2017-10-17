using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
    class ThreadCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new IgnorePropertiesCustomization(new []
            {
                "Messages",
                "Node"
            }));
        }
    }
}
