using AIkailo.Core.Model;
using AIkailo.External.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIkailo.Core.Model
{
    public interface IDataProvider : IDisposable
    {
        void Load(ref Node node);
        IEnumerable<Connection> GetEdges(IEnumerable<Node> activeNodes);
        // TODO: Create Process Node

        void VerifyConnection();
        

        //List<Node> LoadNeighbors(Property property);

        //Concept GetOrCreate(Property property);
        //Scene GetOrCreate(Concept concept1, Concept concept2);
        //Scene GetOrCreate(params Scene[] scenes);
    }
}