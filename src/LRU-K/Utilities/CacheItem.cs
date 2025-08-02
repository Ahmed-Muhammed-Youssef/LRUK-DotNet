using System;
using System.Collections.Generic;

namespace LRU_K.Utilities
{
    internal class CacheItem <TValue>
    {
        public TValue Value { get; set; }
        public List<DateTime> AccessTimes { get; set; }
        public CacheItem(TValue value)
        {
            Value = value;
            AccessTimes = new List<DateTime>();
        }
    }
}
