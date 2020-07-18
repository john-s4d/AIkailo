namespace AIkailo.Internal
{
    /// <summary>
    ///  A weighted ordered pair of Concepts
    /// </summary>
    public class Association : IConceptBase
    {   
        public Concept ParentConcept { get; set; }
        public Concept ChildConcept { get; set; }
        public int Weight { get; set; } = Constants.NEUTRAL;        
    }
}