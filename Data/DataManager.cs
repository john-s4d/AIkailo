using System;
using System.Collections.Generic;
using AIkailo.Internal;

namespace AIkailo.Data
{
    internal class DataManager
    {
        public const int DISK_SECTOR_SIZE = 512;

        private const string ASSOCIATION_DATA_FILENAME = "associations";
        private const string SENSOR_NAME = "Sensor Name";
        private const string ACTION_NAME = "Action Name";

        //ConceptsDataProvider _associationData;

        internal DataManager(string dataDirectory)
        {
            //_associationData = new ConceptsDataProvider(dataDirectory, ASSOCIATION_DATA_FILENAME);

            // _classifyModelData = ?
            // _processModelData = ?
            // _sentimentData = ?

        }

        // Convert SensorMessage data to Scene
        internal Scene Assemble(string sensorName, List<Tuple<string, string>> data)
        {
            Scene result = new Scene();

            Concept cSensorNameLabel = FindOrCreate<Concept>(SENSOR_NAME);
            Concept cSensorNameValue = FindOrCreate<Concept>(sensorName);
            result.AddChild(FindOrCreate<Scene>(cSensorNameLabel, cSensorNameValue));

            foreach (Tuple<string, string> parameter in data)
            {
                Concept cParamName = FindOrCreate<Concept>(parameter.Item1);
                Concept cParamValue = FindOrCreate<Concept>(parameter.Item2);
                result.AddChild(FindOrCreate<Scene>(cParamName, cParamValue));
            }

            return result;
        }

        // Convert Scene to ActionMessage data        
        internal List<Tuple<IConvertible, IConvertible>> Disassemble(out string actionName, Scene scene)
        {
            List<Tuple<IConvertible, IConvertible>> result = new List<Tuple<IConvertible, IConvertible>>();

            actionName = string.Empty;

            foreach (Scene s in scene.Concepts.Keys)
            {
                Concept cParamName = s.Concepts.Keys[0];
                Concept cParamValue = s.Concepts.Keys[1];

                if (cParamName.Definition is string && (string)cParamName.Definition == ACTION_NAME)
                {
                    actionName = (string)cParamValue.Definition;
                }
                else
                {
                    result.Add(new Tuple<IConvertible, IConvertible>(cParamName.Definition, cParamValue.Definition));
                }
            }
            
            return result;
        }

        internal Concept FindOrCreate<T>(IConvertible definition) where T : Concept
        {
            return new Concept()
            {
                Definition = definition,
                Id = null
            };
            //return _associationData.FindOrCreate(item);
            
        }

        private Scene FindOrCreate<T>(params Concept[] concepts) where T : Scene
        {
            Scene scene = new Scene();
            foreach (Concept concept in concepts)
            {
                scene.Concepts.Add(concept, Constants.NEUTRAL);
            }
            return scene;
            //return _associationData.FindOrCreate(concepts);
        }

        // Convert Scene to known Concept patterns with smallest footprint
        internal Scene Reduce(Scene scene)
        {
            return scene;
            //throw new NotImplementedException();
        }

        // Convert Scene to concrete Concept definitions
        internal Scene Expand(Scene scene)
        {
            return scene;
            //throw new NotImplementedException();
        }

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

        // Gets a Scene containing the all the Concepts from the provided scenes, duplicates removed
        internal Scene Union(List<Scene> scenes)
        {
            throw new NotImplementedException();
        }

        // Get a Scene containing the Concepts that appear exactly once in the provided scenes
        internal Scene Except(List<Scene> scenes)
        {
            throw new NotImplementedException();
        }
    }
}
