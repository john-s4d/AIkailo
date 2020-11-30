using System;
using System.Runtime.Serialization;

namespace AIkailo.External.Model
{
    public interface IProperty : IConvertible, ISerializable, IComparable<Property>
    {
    }
}
