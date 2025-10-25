using System;
using System.Collections.Generic;
using SimulationService.Domain.Background;
using SimulationService.Domain.ValueObjects;
using SimulationService.Domain.Enums;
using Xunit;

namespace SimulationService.Tests.Features.Background;

public class InMemorySimulationQueueTests
{
    [Fact]
    public void Enqueue_Dequeue_MaintainsOrder()
    {
        // Arrange
        var queue = new InMemorySimulationQueue();

        var p = new SimulationParams { SeasonYears = new() { "2023/2024" }, LeagueId = Guid.Empty, Iterations = 1, LeagueRoundId = Guid.Empty };
        var s = new SimulationState(SimulationStatus.Pending, 0f, DateTime.UtcNow);

        var job1 = new SimulationJob(Guid.NewGuid(), p, s);
        var job2 = new SimulationJob(Guid.NewGuid(), p, s);
        var job3 = new SimulationJob(Guid.NewGuid(), p, s);

        // Act
        queue.Enqueue(job1);
        queue.Enqueue(job2);
        queue.Enqueue(job3);

        // Assert FIFO order
        Assert.True(queue.TryDequeue(out var out1));
        Assert.Equal(job1, out1);

        Assert.True(queue.TryDequeue(out var out2));
        Assert.Equal(job2, out2);

        Assert.True(queue.TryDequeue(out var out3));
        Assert.Equal(job3, out3);

        // Now empty
        Assert.False(queue.TryDequeue(out var _));
    }

    [Fact]
    public void TryDequeue_OnEmptyQueue_ReturnsFalse()
    {
        var queue = new InMemorySimulationQueue();
        Assert.False(queue.TryDequeue(out var _));
    }
}
