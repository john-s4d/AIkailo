using System;
using AIkailo.Model.Internal;
using AIkailo.Model.Common;

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

        public Concept FindOrCreate(Primitive definition)
        {
            ulong id = 0;

            switch (definition.ToString())
            {
                case "foo":
                    id = 1;
                    break;
                case "bar":
                    id = 2;
                    break;
                case "Interaction.Input":
                    id = 3;
                    break;
                case "Interaction.Output":
                    id = 4;
                    break;
                case "target":
                    id = 5;
                    break;
                case "source":
                    id = 6;
                    break;
            }
            return new Concept() { Definition = definition, Id = id };
            //return FindOrCreate(Constants.DEFAULT_THRESHOLD, definition);
        }

        public Concept FindOrCreate(int threshold, Primitive definition)
        {
            Concept result = _associationData.Find(threshold, definition);
            if (result == null)
            {
                result = _associationData.Create(definition);
            }
            return result;
        }

        public Scene FindOrCreate(params Primitive[] definitions)
        {
            return FindOrCreate(Constants.DEFAULT_THRESHOLD, definitions);
        }

        private ulong id = 0;

        public Scene FindOrCreate(int threshold, params Primitive[] definitions)
        {
            Scene result = new Scene();
            result.Add(new Concept() { Definition = definitions[0], Id = id++ });
            result.Add(new Concept() { Definition = definitions[1], Id = id++ });
            return result;
            /*
            Scene result = _associationData.Find(threshold, definitions);
            if (result == null)
            {
                result = _associationData.Create(definitions);
            }
            return result;*/
        }

        public Scene FindOrCreate(params Concept[] concepts)
        {
            return FindOrCreate(Constants.DEFAULT_THRESHOLD, concepts);
        }

        public Scene FindOrCreate(int threshold, params Concept[] concepts)
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

