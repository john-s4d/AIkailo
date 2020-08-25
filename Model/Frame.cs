namespace AIkailo.Model
{
    /// <summary>
    /// A Progression of Scenes
    /// </summary>
    public class Frame : Concept
    {   
        public Scene Source { get; set; }
        public Scene Target { get; set; }
        public ProcessModel ProcessModel { get; set; }
        public int Sentiment { get; set; } = Constants.NEUTRAL;
    }
}
