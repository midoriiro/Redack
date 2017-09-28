using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture.Kernel;

namespace Redack.DatabaseLayer.Test.Sugar.Specimen
{
    public class IgnoreVirtualProperties : ISpecimenBuilder
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
