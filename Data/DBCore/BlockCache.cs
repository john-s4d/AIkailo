using System;
using System.Collections.Concurrent;

using System.Timers;

namespace AIkailo.Data.DBCore
{
    public class BlockCache : ICache<Block>
    {
        public uint MaxItems { get; set; }
        public uint MaxAgeMs { get; set; }
        public uint IntervalMs { get; }

        private ConcurrentDictionary<int, Block> _blocks;
        private Timer _sweepTimer;

        public BlockCache(uint maxItems, uint maxAgeMs, uint intervalMs)
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

        public void AddOrUpdate(int id, Block item)
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
            _blocks.Clear();
        }

        public void Remove(int id)
        {
            _blocks.TryRemove(id, out Block value);
        }

        public bool TryGet(int id, out Block item)
        {
            return _blocks.TryGetValue(id, out item);
        }
    }
}
