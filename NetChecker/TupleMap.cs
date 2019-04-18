using CsvHelper.Configuration;
using System;

namespace NetChecker
{
    public sealed class TupleMap : ClassMap<ValueTuple<string, string>>
    {
        public TupleMap()
        {
            Map(m => m.Item1).Index(0);
            Map(m => m.Item2).Index(1);
        }
    }
}
