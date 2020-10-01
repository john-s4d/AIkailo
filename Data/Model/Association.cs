namespace AIkailo.Model.Internal
{
    /// <summary>
    ///  A weighted pair of Concepts
    /// </summary>
    public class Association : IAssociation
    {
        public ulong ParentId { get; internal set; }
        public ulong ChildId { get; internal set; }
        public int Weight { get; internal set; }

        internal Association() { }

        internal Association(ulong parentId, ulong childId, int weight)
        {
            ParentId = parentId;
            ChildId = childId;
            Weight = weight;
        }

        internal Association(ulong parentId, ulong childId)
            : this(parentId, childId, Constants.NEUTRAL) { }
    }
}