namespace LRU_K.Tests.Utilities;

public class PriorityQueueTests
{
    [Fact]
    public void EnqueueDequeue_SingleElement_ReturnsCorrectElement()
    {
        // Arrange
        var pq = new LRU_K.Utilities.PriorityQueue<string, int>(10, Comparer<int>.Create((a, b) => a.CompareTo(b)));

        // Act
        pq.UpdateOrEnqueue("A", 1);
        bool dequeued = pq.TryDequeue(out var element, out var priority);

        // Assert
        Assert.True(dequeued);
        Assert.Equal("A", element);
        Assert.Equal(1, priority);
        Assert.Equal(0, pq.Count);
    }

    [Fact]
    public void EnqueueMultiple_DequeueReturnsMinPriority()
    {
        // Arrange
        var pq = new LRU_K.Utilities.PriorityQueue<string, int>(10, Comparer<int>.Create((a, b) => a.CompareTo(b)));
        pq.UpdateOrEnqueue("A", 3);
        pq.UpdateOrEnqueue("B", 1);
        pq.UpdateOrEnqueue("C", 2);

        // Act
        bool dequeued = pq.TryDequeue(out var element, out var priority);

        // Assert
        Assert.True(dequeued);
        Assert.Equal("B", element); // Smallest priority (1)
        Assert.Equal(1, priority);
        Assert.Equal(2, pq.Count);
    }

    [Fact]
    public void UpdateOrEnqueue_ExistingElement_UpdatesPriority()
    {
        // Arrange
        var pq = new LRU_K.Utilities.PriorityQueue<string, int>(10, Comparer<int>.Create((a, b) => a.CompareTo(b)));
        pq.UpdateOrEnqueue("A", 3);

        // Act
        pq.UpdateOrEnqueue("A", 1); // Update A's priority to 1
        bool dequeued = pq.TryDequeue(out var element, out var priority);

        // Assert
        Assert.True(dequeued);
        Assert.Equal("A", element);
        Assert.Equal(1, priority); // Updated priority
        Assert.Equal(0, pq.Count);
    }

    [Fact]
    public void TryDequeue_EmptyQueue_ReturnsFalse()
    {
        // Arrange
        var pq = new LRU_K.Utilities.PriorityQueue<string, int>(10, Comparer<int>.Create((a, b) => a.CompareTo(b)));

        // Act
        bool dequeued = pq.TryDequeue(out var element, out var priority);

        // Assert
        Assert.False(dequeued);
        Assert.Null(element);
        Assert.Equal(0, priority);
    }

    [Fact]
    public void UpdateOrEnqueue_MultipleUpdates_MaintainsHeapProperty()
    {
        // Arrange
        var pq = new LRU_K.Utilities.PriorityQueue<string, int>(10, Comparer<int>.Create((a, b) => a.CompareTo(b)));
        pq.UpdateOrEnqueue("A", 5);
        pq.UpdateOrEnqueue("B", 3);
        pq.UpdateOrEnqueue("C", 7);

        // Act
        pq.UpdateOrEnqueue("A", 2); // Update A's priority to 2
        bool dequeued = pq.TryDequeue(out var element, out var priority);

        // Assert
        Assert.True(dequeued);
        Assert.Equal("A", element); // A now has smallest priority (2)
        Assert.Equal(2, priority);
        Assert.Equal(2, pq.Count);
    }
}
