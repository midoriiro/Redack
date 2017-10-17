using System.Linq;
using Ploeh.AutoFixture;

namespace Redack.Test.Lollipop.Customizations
{
    public class OmitOnRecursionCustomization : ICustomization
    {
        private readonly int _recursionDepth;

        public OmitOnRecursionCustomization(int recursionDepth = 1)
        {
            this._recursionDepth = recursionDepth;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList().ForEach(x => fixture.Behaviors.Remove(x));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior(this._recursionDepth));
        }
    }
}
