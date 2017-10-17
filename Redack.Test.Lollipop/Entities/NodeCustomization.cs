using Ploeh.AutoFixture;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
    class NodeCustomization : BaseEntityCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);

            fixture.Customize(new IgnorePropertiesCustomization(new []
            {
                "Threads"
            }));
            fixture.Customize(new StringMaxLengthCustomization<Node>("Name", 30));
        }
    }
}
