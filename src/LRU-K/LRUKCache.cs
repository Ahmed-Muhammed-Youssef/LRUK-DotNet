using LRU_K.Utilities;
using System;
using System.Collections.Generic;

namespace LRU_K
{
    /// <summary>
    /// Implements an LRU-K cache that evicts items based on their Kth most recent access time.
    /// Uses a min-heap to track eviction order and a dictionary for O(1) key-value lookups.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the cache.</typeparam>
    /// <typeparam name="TValue">The type of the values in the cache.</typeparam>
    public class LRUKCache<TKey, TValue>
    {
        private readonly int _capacity;
        private readonly int _k;
        private readonly Dictionary<TKey, CacheItem<TValue>> _cache;
        private readonly PriorityQueue<TKey, DateTime> _priorityQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LRUKCache{TKey, TValue}"/> class with the specified capacity and K value.
        /// </summary>
        /// <param name="capacity">The maximum number of items the cache can hold. Must be non-negative.</param>
        /// <param name="k">The number of recent accesses to track for eviction. Must be positive.</param>
        public LRUKCache(int capacity, int k)
        {
            _capacity = capacity;
            _k = k;
            _cache = new Dictionary<TKey, CacheItem<TValue>>(capacity);
            _priorityQueue = new PriorityQueue<TKey, DateTime>(capacity, Comparer<DateTime>.Create((a,b) => a.CompareTo(b)));
        }

        /// <summary>
        /// Retrieves the value associated with the specified key and updates its access time.
        /// </summary>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the key, or the default value for <typeparamref name="TValue"/> if the key is not found.</returns>
        public TValue Get(TKey key)
        {
            if (!_cache.TryGetValue(key, out var item))
                return default!;

            UpdateAccessTime(key, item);
            return item.Value;
        }

        /// <summary>
        /// Adds or updates a key-value pair in the cache and updates its access time.
        /// Evicts the least recently used item if the cache is full.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to associate with the key.</param>
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
