# LRU-K Cache

A C# implementation of an LRU-K cache, which evicts items based on their Kth most recent access time, using a custom priority queue for efficient eviction.

## Features
- Generic `LRUKCache<TKey, TValue>` with configurable capacity and K value.
- O(log n) operations for `Get`, `Put`, and eviction using a min-heap (`PriorityQueue<TElement, TPriority>`).
- Supports priority updates to avoid duplicate entries in the heap.
- Unit tests with xUnit to verify cache and priority queue behavior.

## Setup
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Ahmed-Muhammed-Youssef/LRUK-DotNet
   ```

2. **Build**:
   ```bash
   dotnet build
   ```

3. **Run Tests**:
   ```bash
   dotnet test
   ```

## Usage
```csharp
var cache = new LRUKCache<string, string>(capacity: 2, k: 2);
cache.Put("A", "Apple");
cache.Put("B", "Banana");
Console.WriteLine(cache.Get("A")); // Outputs: Apple
cache.Put("C", "Cherry"); // Evicts B (fewer or older Kth access)
Console.WriteLine(cache.Get("B")); // Outputs: null
```

## Requirements
- .NET Core 3.1 or later.
- xUnit for testing.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
