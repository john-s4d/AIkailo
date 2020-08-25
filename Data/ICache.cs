namespace AIkailo.Data
{
    public interface ICache<T>
    {   
        uint MaxItems { get; set; }
        uint MaxAgeMs { get; set; }
        uint IntervalMs { get; }
        void SetSweepInterval(uint intervalMs);
        bool TryGet(int id, out T item);
        void AddOrUpdate(int id, T item);
        void Remove(int id);
        void Flush();
    }
}
