using System;
using System.Collections.Generic;
using AIkailo.External.Model;
using QuikGraph;

namespace AIkailo.Core.Model
{
    public interface IConceptEdge
        : IEdge<IConcept>, ITagged<IDictionary<Property, Property>>
    { }
}