using AIkailo.Core.Model;
using AIkailo.Data;
using AIkailo.External.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    public class SceneProvider : ISceneProvider
    {

        IDataProvider _dataProvider;
        public SceneProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Dispose()
        {
            _dataProvider.Dispose();
        }

        public Concept New(Property property)
        {
            return _dataProvider.GetOrCreate(property);
        }

        public Scene New(Concept concept)
        {
            throw new NotImplementedException();
        }

        public Scene New(Property property1, Property property2)
        {
            Concept c1 = _dataProvider.GetOrCreate(property1);
            Concept c2 = _dataProvider.GetOrCreate(property2);
            return _dataProvider.GetOrCreate(c1, c2);
        }

        public Scene New(params Scene[] scenes)
        {
            return _dataProvider.GetOrCreate(scenes);
        }

        public Scene New(params Concept[] concepts)
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
