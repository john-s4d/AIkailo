namespace AIkailo.Model.Internal
{
    /// <summary>
    ///  A weighted pair of Concepts
    /// </summary>
    public class Association
    {
        public ulong ParentId { get; set; }
        public ulong ChildId { get; set; }
        public int Weight { get; set; }

        private Association() { }

        public Association(ulong parentId, ulong childId, int weight)
        {
            ParentId = parentId;
            ChildId = childId;
            Weight = weight;
        }

        public Association(ulong parentId, ulong childId)
            : this(parentId, childId, Constants.NEUTRAL) { }
    }
}