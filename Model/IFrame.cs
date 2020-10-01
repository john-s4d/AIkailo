namespace AIkailo.Model.Internal
{
    /// <summary>
    /// A Progression of Scenes
    /// </summary>
    public interface IFrame : IConcept
    {   
        IScene Source { get; set; }
        IScene Target { get; set; }
        IProcessModel ProcessModel { get; set; }
        int Sentiment { get; set; }
    }
}
