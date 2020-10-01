using AIkailo.Common;

namespace AIkailo.Model.Internal
{
    /// <summary>
    ///  The basic block
    /// </summary>
    public class Concept : IConcept
    {
        internal Concept() { }

        internal Concept(Primitive definition, ulong id)
        {
            Definition = definition;
            Id = id;
        }

        public ulong Id { get; internal set; }
        public Primitive Definition { get; internal set; }

        public int CompareTo(object obj)
        {
            Concept c = obj as Concept;
            if (Id < c.Id) { return -1; }
            if (Id > c.Id) { return 1; }
            return 0;
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
        }
    }
}