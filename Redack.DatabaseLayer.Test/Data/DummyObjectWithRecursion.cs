using Redack.DatabaseLayer.Test.Sugar.Data;

namespace Redack.DatabaseLayer.Test.Data
{
    public class DummyObjectWithRecursion : DummyObject
    {
        public virtual DummyObjectWithRecursion Property3 { get; set; }
    }
}