using System;
using System.Collections;
using System.Collections.Generic;

namespace AIkailo.Model
{
    /// <summary>
    ///  An ordered and weighted list of Associations, where this Scene is the parent Concept. 
    /// </summary>
    public class Scene : Concept, IReadOnlyList<Association>
    {
        // TODO: Performance of sorted array vs sort array on demand
        //private SortedDictionary<Concept, Association> _associations; // SortedDictionary is O(log n) read/write
        private SortedList<Concept, Association> _associations; // SortedList has indexing so we can implement the readonlylist

        public void Add(Scene scene, int weight = Constants.NEUTRAL)
        {
            _associations.Add(scene, new Association(this, scene, weight));
        }

        public void Add(Concept concept, int weight = Constants.NEUTRAL)
        {
            _associations.Add(concept, new Association(this, concept, weight));
        }

        public IEnumerator<Association> GetEnumerator()
        {
            return _associations.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _associations.GetEnumerator();
        }

        public Association this[int index] 
        {
            get { return _associations.Values[index]; }
        } 

        public int Count
        {
            get { return _associations.Count; }
        }
    }
}