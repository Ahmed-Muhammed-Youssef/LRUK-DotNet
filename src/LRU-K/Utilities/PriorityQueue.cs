using System.Collections.Generic;

namespace LRU_K.Utilities
{
    /// <summary>
    /// Custom min-heap implementation for tracking Kth access times with priority updates
    /// Ensures each element appears only once by updating priorities instead of adding duplicates
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TPriority"></typeparam>
    internal class PriorityQueue <TElement, TPriority>
    {
        private readonly List<(TElement Element, TPriority Priority)> _data;
        private readonly IComparer<TPriority> _priorityComparer;
        private readonly Dictionary<TElement, int> _elementToIndex;

        public PriorityQueue(int capacity, IComparer<TPriority> priorityComparer)
        {
            _data = new List<(TElement, TPriority)>(capacity);
            _priorityComparer = priorityComparer;
            _elementToIndex = new Dictionary<TElement, int>();
        }

        public int Count => _data.Count;

        // Adds a new element or updates an existing element's priority
        public void UpdateOrEnqueue(TElement element, TPriority priority)
        {
            if (_elementToIndex.TryGetValue(element, out int index))
            {
                UpdatePriorityAtIndex(index, priority);
            }
            else
            {
                Enqueue(element, priority);
            }
        }

        // Adds a new element to the heap
        private void Enqueue(TElement element, TPriority priority)
        {
            _data.Add((element,priority));

            int i = _data.Count - 1;

            _elementToIndex[element] = i;
            while (i > 0)
            {
                int parentIndex = (i - 1)/2;

                if (_priorityComparer.Compare(_data[i].Priority, _data[parentIndex].Priority) >= 0) break;

                (_data[i], _data[parentIndex]) = (_data[parentIndex], _data[i]);
                _elementToIndex[_data[i].Element] = i;
                _elementToIndex[_data[parentIndex].Element] = parentIndex;
                i = parentIndex;
            }
        }

        // Removes and returns the element with the lowest priority (root)
        public bool TryDequeue(out TElement element, out TPriority priority)
        {
            if(_data.Count == 0)
            {
                element = default!;
                priority = default!;
                return false;
            }

            element = _data[0].Element;
            priority = _data[0].Priority;

            _data[0] = _data[^1];
            _data.RemoveAt(_data.Count - 1);
            _elementToIndex.Remove(element);

            if (_data.Count > 0)
            {
                _elementToIndex[_data[0].Element] = 0;
                BubbleDown(0);
            }

            return true;
        }

        // Updates the priority of an element at the given index and adjusts its position
        private void UpdatePriorityAtIndex(int index, TPriority newPriority)
        {
            TPriority oldPriority = _data[index].Priority;
            _data[index] = (_data[index].Element, newPriority);

            // Compare old and new priorities to decide whether to bubble up or down
            if (_priorityComparer.Compare(newPriority, oldPriority) < 0)
            {
                BubbleUp(index);
            }
            else if (_priorityComparer.Compare(newPriority, oldPriority) > 0)
            {
                BubbleDown(index);
            }
        }

        // Bubbles up the element at the given index to maintain min-heap property
        private void BubbleUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (_priorityComparer.Compare(_data[i].Priority, _data[parent].Priority) >= 0)
                    break;
                (_data[i], _data[parent]) = (_data[parent], _data[i]);
                _elementToIndex[_data[i].Element] = i;
                _elementToIndex[_data[parent].Element] = parent;
                i = parent;
            }
        }

        // Bubbles down the element at the given index to maintain min-heap property
        private void BubbleDown(int i)
        {
            while (true)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;

                if (left < _data.Count && _priorityComparer.Compare(_data[left].Priority, _data[smallest].Priority) < 0)
                    smallest = left;
                if (right < _data.Count && _priorityComparer.Compare(_data[right].Priority, _data[smallest].Priority) < 0)
                    smallest = right;

                if (smallest == i)
                    break;

                (_data[i], _data[smallest]) = (_data[smallest], _data[i]);
                _elementToIndex[_data[i].Element] = i;
                _elementToIndex[_data[smallest].Element] = smallest;
                i = smallest;
            }
        }

    }
}
