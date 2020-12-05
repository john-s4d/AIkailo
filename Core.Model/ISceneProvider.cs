using AIkailo.Core.Model;
using AIkailo.External.Model;

namespace AIkailo.Data
{
    public interface ISceneProvider
    {
        Concept New(Property property);

        Scene New(Concept concept);

        Scene New(Property property1, Property property2);

        Scene New(params Concept[] concept);

        Scene New(params Scene[] scenes);

    }
}