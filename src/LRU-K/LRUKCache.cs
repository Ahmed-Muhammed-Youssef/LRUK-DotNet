using LRU_K.Utilities;
using System;
using System.Collections.Generic;

namespace LRU_K
{
    public class LRUKCache<TKey, TValue>
    {
        private readonly int _capacity;
        private readonly int _k;
        private readonly Dictionary<TKey, CacheItem<TValue>> _cache;
        private readonly PriorityQueue<TKey, DateTime> _priorityQueue;

        public LRUKCache(int capacity, int k)
        {
            _capacity = capacity;
            _k = k;
            _cache = new Dictionary<TKey, CacheItem<TValue>>(capacity);
            _priorityQueue = new PriorityQueue<TKey, DateTime>(capacity, Comparer<DateTime>.Create((a,b) => a.CompareTo(b)));
        }

        public TValue Get(TKey key)
        {
            if (!_cache.TryGetValue(key, out var item))
                return default!;

            UpdateAccessTime(key, item);
            return item.Value;
        }

        public void Put(TKey key, TValue value)
        {
            if (_cache.ContainsKey(key))
            {
                var item = _cache[key];
                item.Value = value;
                UpdateAccessTime(key, item);
                return;
            }
            if (_cache.Count >= _capacity)
                Evict();
            var newItem = new CacheItem<TValue>(value);
            _cache[key] = newItem;
            UpdateAccessTime(key, newItem);
        }

        private void Evict()
        {
            if (_priorityQueue.TryDequeue(out var key, out _))
            {
                _cache.Remove(key);
            }
        }
        private void UpdateAccessTime(TKey key, CacheItem<TValue> item)
        {
            item.AccessTimes.Add(DateTime.Now);
            if (item.AccessTimes.Count > _k)
                item.AccessTimes.RemoveAt(0);

            DateTime kthAccessTime = item.AccessTimes.Count >= _k
                ? item.AccessTimes[^_k]
                : DateTime.MinValue;

            _priorityQueue.UpdateOrEnqueue(key, kthAccessTime);
        }
    }
}
