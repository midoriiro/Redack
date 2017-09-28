using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;

namespace Redack.DatabaseLayer.Test.Sugar.Customization
{
    class OmitOnRecursionCustomization : ICustomization
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
