namespace AIkailo.Model.Internal
{
    /// <summary>
    ///  A weighted pair of Concepts
    /// </summary>
    public interface IAssociation
    {
        ulong ParentId { get; }
        ulong ChildId { get; }
        int Weight { get; }

    }
}