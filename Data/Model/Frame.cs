namespace AIkailo.Model.Internal
{
    /// <summary>
    /// A Progression of Scenes
    /// </summary>
    public class Frame : Concept
    {   
        public Scene Source { get; internal set; }
        public Scene Target { get; internal set; }
        public ProcessModel ProcessModel { get; internal set; }
        public int Sentiment { get; internal set; } = Constants.NEUTRAL;
    }
}
