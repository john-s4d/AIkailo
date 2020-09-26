using AIkailo.Model.Common;
using System;

namespace AIkailo.Model.Internal
{
    public interface IConcept : IConceptBase, IComparable
    {
        ulong Id { get; set; }
        Primitive Definition { get; set; }      
    }
}