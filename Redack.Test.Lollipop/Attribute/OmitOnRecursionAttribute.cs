using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Attribute
{
    class OmitOnRecursionAttribute : AutoDataAttribute
    {
        public OmitOnRecursionAttribute(int recursionDepth = 1) :
            base(new Fixture().Customize(new OmitOnRecursionCustomization(recursionDepth)))
        {
        }
    }
}
