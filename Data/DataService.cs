using System;
using AIkailo.Model.Internal;
using AIkailo.Common;
using System.Linq;

namespace AIkailo.Data
{
    public sealed class DataService : IAIkailoService
    {
        public string Name { get; } = "AIkailo.DataService";

        public IAkailoServiceState State => throw new NotImplementedException();

        private AssociationDataProvider _associationData;

        public DataService(string dataDirectory)
        {
            _associationData = new AssociationDataProvider(dataDirectory);
            // _classifyModelData = ?
            // _processModelData = ?
            // _sentimentData = ?
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        // Exact Matching

        public IConcept FindOrCreate(Primitive definition)
        {
            return _associationData.Find(definition) ?? _associationData.Create(definition);
        }

        public IScene FindOrCreate(params Primitive[] definitions)
        {   
            return FindOrCreate(definitions.Select(x => FindOrCreate(x)).ToArray());
        }

        public IScene FindOrCreate(params PrimitivePair[] pairs)
        {
            return FindOrCreate(pairs.Select(x => FindOrCreate(x.Item1, x.Item2)).ToArray());
        }
        
        public IScene FindOrCreate(params IConcept[] concepts)
        {
            return _associationData.Find(concepts) ?? _associationData.Create(concepts);
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

