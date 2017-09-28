using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redack.DatabaseLayer.Test.Sugar
{
    public class DummyObject
    {
        public string Property1 { get; set; }
        public virtual string Property2 { get; set; }
    }

    public class DummyObjectWithRecursion : DummyObject
    {
        public virtual DummyObjectWithRecursion Property3 { get; set; }
    }
}
