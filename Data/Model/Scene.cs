using System;
using System.Collections;
using System.Collections.Generic;

namespace AIkailo.Model.Internal
{
    /// <summary>
    ///  An ordered and weighted list of Associations, where this Scene is the parent Concept. 
    /// </summary>
    public class Scene : Concept, IReadOnlyList<Association>
    {
        // TODO: Performance of sorted array vs sort array on demand
        //private SortedDictionary<Concept, Association> _associations; // SortedDictionary is O(log n) read/write
        private readonly SortedList<Concept, Association> _associations;

        internal Scene()
        {
            _associations = new SortedList<Concept, Association>(); // SortedList has indexing so we can implement the readonlylist
        }
        internal void Add(params Concept[] concepts)
        {
            foreach (Concept c in concepts)
            {
                Add(c, Constants.NEUTRAL);
            }
        }

        internal void Add(Scene scene, int weight = Constants.NEUTRAL)
        {
            _associations.Add(scene, new Association(this.Id, scene.Id, weight));
        }

        internal void Add(Concept concept, int weight = Constants.NEUTRAL)
        {
            _associations.Add(concept, new Association(this.Id, concept.Id, weight));
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