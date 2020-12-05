using AIkailo.Core.Model;
using AIkailo.External.Model;

namespace AIkailo.Data
{
    public interface IDataProvider
    {
        Concept GetOrCreate(Property property);
        Scene GetOrCreate(Concept concept1, Concept concept2);
        Scene GetOrCreate(params Scene[] scenes);
    }
}