using AIkailo.External.Model;
using QuikGraph;
using System;
using System.Collections.Generic;

namespace AIkailo.Core.Model
{
    public class ConceptEdge : TaggedEdge<Concept, Dictionary<Property, Property>>
    {
        public ConceptEdge(Concept source, Concept target, Dictionary<Property, Property> tags = null) 
            : base(source, target, tags) 
        { }

        public ConceptEdge(Concept source, Concept target, IReadOnlyDictionary<string, object> properties) 
            : base(source, target, null)
        {
            Tag = new Dictionary<Property, Property>();
            foreach (string key in properties.Keys)
            {
                Tag.Add(key, (string)properties[key]);
            }
        }

        public ConceptEdge(Concept source, Concept target, Property tag, Property value) 
            : base(source, target, null)
        {
            Tag = new Dictionary<Property, Property>();
            Tag.Add(tag, value);
        }
    }
}