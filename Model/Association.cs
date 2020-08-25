namespace AIkailo.Model
{
    /// <summary>
    ///  A weighted pair of Concepts
    /// </summary>
    public class Association
    {
        public Concept Parent { get; set; }
        public Concept Child { get; set; }
        public int Weight { get; set; }

        private Association() { }

        public Association(Concept parent, Concept child, int weight)
        {
            Parent = parent;
            Child = child;
            Weight = weight;
        }

        public Association(Concept parent, Concept child)
            : this(parent, child, Constants.NEUTRAL) { }
    }
}