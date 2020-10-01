using AIkailo.Common;
using System;

namespace AIkailo.Model.Internal
{
    public interface IConcept : IConceptBase, IComparable
    {
        ulong Id { get; }
        Primitive Definition { get; }      
    }
}