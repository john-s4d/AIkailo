using System;
using System.Collections.Generic;
using AIkailo.Model.Internal;

namespace AIkailo.Data
{
    internal class DataManager
    {
        private AssociationDataProvider _associationData;

        internal DataManager(string dataDirectory)
        {
            _associationData = new AssociationDataProvider(dataDirectory);
            // _classifyModelData = ?
            // _processModelData = ?
            // _sentimentData = ?
        }
        
        internal Concept FindOrCreate(IConvertible definition)
        {
            return FindOrCreate(Constants.DEFAULT_THRESHOLD, definition);
        }

        internal Concept FindOrCreate(int threshold, IConvertible definition)
        {
            Concept result = _associationData.Find(threshold, definition);
            if (result == null)
            {
                result = _associationData.Create(definition);
            }
            return result;
        }

        internal Scene FindOrCreate(params Concept[] concepts)
        {
            return FindOrCreate(Constants.DEFAULT_THRESHOLD, concepts);
        }

        internal Scene FindOrCreate(int threshold, params Concept[] concepts)
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