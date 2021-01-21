using AIkailo.Core.Model;
using AIkailo.External.Model;
using System;

namespace AIkailo.Data
{
    public interface IDataProvider : IDisposable
    {
        Concept GetOrCreate(Property property);
        Scene GetOrCreate(Concept concept1, Concept concept2);
        Scene GetOrCreate(params Scene[] scenes);
    }
}