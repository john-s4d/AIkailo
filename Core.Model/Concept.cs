using AIkailo.External.Model;
using System;

namespace AIkailo.Core.Model
{
    public class Concept : Property
    {
        public IConvertible Definition { get; set; }
        public string Id { get; set; }

        public Concept() : 
            base() 
        { }

        public Concept(Property definition, string id = null)
            : base(definition)
        {
            Definition = definition;
            Id = id;
        }
    }
}