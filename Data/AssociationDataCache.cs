using AIkailo.Model;
using System;
using System.Collections.Concurrent;

using System.Timers;

namespace AIkailo.Data
{
    public class AssociationDataCache //: ICache<Block>
    {
        public uint MaxItems { get; set; }
        public uint MaxAgeMs { get; set; }
        public uint IntervalMs { get; }

        // conceptId => Concept Struct
        private ConcurrentDictionary<ulong, ConceptDbRecord> _conceptDbCache;

        private Timer _sweepTimer;

        public AssociationDataCache(uint maxItems, uint maxAgeMs, uint intervalMs)
        {
            MaxItems = maxItems;
            MaxAgeMs = maxAgeMs;
            SetSweepInterval(intervalMs);
        }

        private void _sweepTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SetSweepInterval(uint interval)
        {
            if (interval > 0)
            {
                if (_sweepTimer == null)
                {
                    _sweepTimer = new Timer();
                    _sweepTimer.Elapsed += _sweepTimer_Elapsed;
                }
                _sweepTimer = new Timer(interval);                
                _sweepTimer.Start();
            }
            else
            {
                _sweepTimer?.Stop();
            }
        }

        public void AddOrUpdate(int id, Concept item)
        {
            throw new NotImplementedException();
            /*

            _blocks.AddOrUpdate(id, item, Merge_Block)
            if (_blocks.ContainsKey(id))
            {
                _blocks[id] = item;
            }
            else
            {
                _blocks.Add.Add(id, item);
            }*/
        }

        public void Flush()
        {
            _conceptDbCache.Clear();
        }

        public void Remove(ulong id)
        {
            _conceptDbCache.TryRemove(id, out ConceptDbRecord value);
        }

        public bool TryGet(ulong id, out ConceptDbRecord item)
        {
            return _conceptDbCache.TryGetValue(id, out item);
        }
    }
}
