using System;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Model
{
    public interface IConcept : IConceptBase, IComparable
    {
        ulong? Id { get; set; }
        IConvertible Definition { get; set; }      
    }
}