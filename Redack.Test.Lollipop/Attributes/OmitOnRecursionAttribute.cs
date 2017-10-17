using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Attributes
{
    class OmitOnRecursionAttribute : AutoDataAttribute
    {
        public OmitOnRecursionAttribute(int recursionDepth = 1) :
            base(new Fixture().Customize(new OmitOnRecursionCustomization(recursionDepth)))
        {
        }
    }
}
