using System;
using System.Collections.Generic;
using AIkailo.Internal;

namespace AIkailo.Data
{
    internal static class SceneExtension
    {   
        public static void AddChild(this Scene parent, Concept child)
        {
            parent.Concepts.Add(child, Constants.NEUTRAL);
        }
    }
}
