using AIkailo.External.Model;
using System;
using System.Collections.Generic;

namespace AIkailo.Core.Model
{
    /// <summary>
    ///  A weighted pair of Concepts
    /// </summary>
    public class ConceptEdge : IConceptEdge
    {
        public IConcept Source => throw new NotImplementedException();

        public IConcept Target => throw new NotImplementedException();

        public IDictionary<Property, Property> Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler TagChanged;
    }
}