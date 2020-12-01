using AIkailo.Core.Model;
using AIkailo.External.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    internal class SceneFactory
    {
        internal static Concept New(Property p)
        {
            return new Concept(p);
        }

        internal static Scene New(params Property[] properties)
        {
            /*Scene result = new Scene();
            result.Target = new Concept();
            foreach (Property p in properties)
            {
                ConceptEdge ce = new ConceptEdge();
                ce.Source = p;
                ce.
            }*/
            throw new NotImplementedException();
        }

        internal static Scene New(params Feature[] f)
        {
            throw new NotImplementedException();
        }

        internal static Scene New(params FeatureVector[] v)
        {
            throw new NotImplementedException();
        }
        internal static Scene New(params Concept[] c)
        {
            throw new NotImplementedException();
        }

        internal static Scene New(params ConceptEdge[] d)
        {
            throw new NotImplementedException();
        }        

        internal static Scene New(params Scene[] s)
        {
            
            
            throw new NotImplementedException();
        }

        /*
        // Find and rank similar Scenes
        internal List<Scene> Similar(Scene scene, int threshold)
        {
            throw new NotImplementedException();
        }

        // Reorder the candidates according to rank
        internal void Rank(ref List<Scene> candidates, Scene sample)
        {
            throw new NotImplementedException();
        }

        // Get an intersection of the provided Scenes
        internal Scene Intersect(List<Scene> scenes)
        {
            throw new NotImplementedException();
        }

        // Generate a Scene containing the all the Concepts from the provided Scenes, duplicates removed
        internal Scene Union(List<Scene> scenes)
        {
            throw new NotImplementedException();
        }

        // Generate a Scene containing the Concepts that appear exactly once in the provided Scenes
        internal Scene Unique(List<Scene> scenes)
        {
            throw new NotImplementedException();
        }

        internal Scene Except(List<Scene> scenes)
        {
            throw new NotImplementedException();
        }
*/
    }
}
