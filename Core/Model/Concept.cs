using AIkailo.External.Model;
using System;

namespace AIkailo.Core.Model
{
    /// <summary>
    ///  The basic block
    /// </summary>
    public class Concept : Property, IConcept
    {
        /*
        internal Concept() { }

        internal Concept(Property definition, ulong id)
        {
            Definition = definition;
            Id = id;
        }

        public ulong Id { get; internal set; }
        public Property Definition { get; internal set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Concept otherConcept = obj as Concept;
            if (otherConcept != null)
                return Id.CompareTo(otherConcept.Id);
            else
                throw new ArgumentException("Object is not a Concept");
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Concept c = obj as Concept;

            return c.Id != null && c.Id == Id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }*/
    }
}