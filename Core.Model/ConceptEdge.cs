using AIkailo.External.Model;
using QuikGraph;
using System;
using System.Collections.Generic;

namespace AIkailo.Core.Model
{
    public class ConceptEdge : IEdge<Concept>, ITagged<Dictionary<Property, Property>>
    //: IConceptEdge
    {
        public Dictionary<Property, Property> Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Concept Source => throw new NotImplementedException();

        public Concept Target => throw new NotImplementedException();

        public event EventHandler TagChanged;
    }
}