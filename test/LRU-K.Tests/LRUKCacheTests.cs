namespace LRU_K.Tests;

public class LRUKCacheTests
{
    [Fact]
    public void Get_MissingKey_ReturnsDefault()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(2, 2);

        // Act
        var value = cache.Get("A");

        // Assert
        Assert.Null(value);
    }

    [Fact]
    public void PutAndGet_SingleItem_ReturnsCorrectValue()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(2, 2);

        // Act
        cache.Put("A", "Apple");
        var value = cache.Get("A");

        // Assert
        Assert.Equal("Apple", value);
    }

    [Fact]
    public void Put_FullCache_EvictsOldestKthAccess()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(2, 2);
        cache.Put("A", "Apple");
        cache.Put("B", "Banana");
        // Simulate time passing for A and B
        System.Threading.Thread.Sleep(10);
        cache.Get("A"); // 1st access for A
        System.Threading.Thread.Sleep(10);
        cache.Get("A"); // 2nd access for A (K=2, recent Kth access)
        System.Threading.Thread.Sleep(10);
        cache.Get("B"); // 1st access for B (older Kth access)

        // Act
        cache.Put("C", "Cherry"); // Should evict B (fewer or older Kth access)

        // Assert
        Assert.Null(cache.Get("B"));
        Assert.Equal("Apple", cache.Get("A"));
        Assert.Equal("Cherry", cache.Get("C"));
    }

    [Fact]
    public void UpdateValue_ExistingKey_UpdatesValueAndAccessTime()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(2, 2);
        cache.Put("A", "Apple");

        // Act
        cache.Put("A", "Apricot");
        var value = cache.Get("A");

        // Assert
        Assert.Equal("Apricot", value);
    }

    [Fact]
    public void KAccesses_RespectsKthAccessTime()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(2, 2);
        cache.Put("A", "Apple");
        cache.Put("B", "Banana");
        // Simulate A's accesses to reach K=2
        System.Threading.Thread.Sleep(10);
        cache.Get("A"); // 1st access
        System.Threading.Thread.Sleep(10);
        cache.Get("A"); // 2nd access (Kth access time set)
                        // B has only 1 access, with MinValue as Kth access
        System.Threading.Thread.Sleep(10);
        cache.Get("B"); // 1st access

        // Act
        cache.Put("C", "Cherry"); // Should evict B (MinValue < A's Kth access time)

        // Assert
        Assert.Null(cache.Get("B"));
        Assert.Equal("Apple", cache.Get("A"));
        Assert.Equal("Cherry", cache.Get("C"));
    }

    [Fact]
    public void EmptyCache_PutAndGet_WorksCorrectly()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(1, 1);

        // Act
        cache.Put("A", "Apple");
        var value = cache.Get("A");

        // Assert
        Assert.Equal("Apple", value);
        Assert.Null(cache.Get("B"));
    }

    [Fact]
    public void FullCache_MultipleUpdates_EvictsCorrectly()
    {
        // Arrange
        var cache = new LRUKCache<string, string>(2, 2);
        cache.Put("A", "Apple");
        cache.Put("B", "Banana");
        // Update A's access times
        System.Threading.Thread.Sleep(10);
        cache.Get("A"); // 1st access
        System.Threading.Thread.Sleep(10);
        cache.Get("A"); // 2nd access (K=2, sets Kth access time)
                        // Update B's access time once
        System.Threading.Thread.Sleep(10);
        cache.Get("B"); // 1st access (Kth access = MinValue)

        // Act
        cache.Put("C", "Cherry"); // Should evict B (MinValue < A's Kth access)
        cache.Put("D", "Date");   // Should evict C (MinValue < A's Kth access)

        // Assert
        Assert.Equal("Apple", cache.Get("A"));
        Assert.Null(cache.Get("B"));
        Assert.Null(cache.Get("C"));
        Assert.Equal("Date", cache.Get("D"));
    }
}
