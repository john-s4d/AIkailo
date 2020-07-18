using System;

namespace AIkailo.Internal
{
    public interface IConcept : IConceptBase, IComparable
    {
        ulong? Id { get; set; }
        IConvertible Definition { get; set; }      
    }
}