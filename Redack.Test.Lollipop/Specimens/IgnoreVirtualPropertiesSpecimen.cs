using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Redack.Test.Lollipop.Specimens
{
    public class IgnoreVirtualPropertiesSpecimen : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            PropertyInfo propertyInfo = request as PropertyInfo;

            if(propertyInfo != null && propertyInfo.GetGetMethod().IsVirtual)
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
