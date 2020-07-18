using System;

namespace AIkailo.Internal
{
    /// <summary>
    ///  The basic block
    /// </summary>
    public class Concept : IConcept
    {
        public ulong? Id { get; set; }
        public IConvertible Definition { get; set; }

        public int CompareTo(object obj)
        {
            Concept c = obj as Concept;
            if (Id < c.Id) { return -1; }            
            if (Id > c.Id) { return 1; }
            return 0;            
        }
    } 
}