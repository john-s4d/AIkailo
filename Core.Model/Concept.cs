using AIkailo.External.Model;
using System;

namespace AIkailo.Core.Model
{
    public class Concept : Property //, IConcept
    {
        public Concept(Property p) 
            : base(p)
        { }

        public Concept() : base() { }

    }
}