using System;

namespace AIkailo.Model
{
    public class DataPair<T1, T2>
        where T1 : IConvertible
        where T2 : IConvertible
    {
        T1 Item1 { get; set; }
        T2 Item2 { get; set; }

        public DataPair(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
            
    }
}
