using System;
using System.Collections;
using System.Collections.Generic;

namespace AIkailo.Internal
{
    /// <summary>
    ///  An ordered and weighted list of Concepts
    /// </summary>
    public class Scene : Concept // , IEnumerable<Scene>
    {
        //public SortedList<IConcept, int> ParentScenes { get; set; } // ?? Should we have both ways?? maybe for efficient routing.. Data is available
        // TODO: SceneContainer to make traversing Scenes and concepts easier

        public SortedList<Concept, int> Concepts { get; set; }
       
    }
}