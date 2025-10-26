using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimulationService.Domain.Background;
using SimulationService.Domain.ValueObjects;
using SimulationService.Domain.Enums;
using Xunit;
using SimulationService.Domain.Entities;

namespace SimulationService.Tests.Features.Background
{
    public class InMemorySimulationQueueTests
    {
        [Fact]
        public async Task EnqueueAsync_DequeueAsync_MaintainsOrder()
        {
            // Arrange
            var queue = new InMemorySimulationQueue();

            var id = Guid.NewGuid();

            var p = new SimulationParams
            {
                SeasonYears = new() { "2023/2024" },
                LeagueId = Guid.Empty,
                Iterations = 1,
                LeagueRoundId = Guid.Empty
            };

            var state = new SimulationState(id, 0, 0f, SimulationStatus.Pending, DateTime.UtcNow);

            var job1 = new SimulationJob(Guid.NewGuid(), p, state);
            var job2 = new SimulationJob(Guid.NewGuid(), p, state);
            var job3 = new SimulationJob(Guid.NewGuid(), p, state);

            // Act
            await queue.EnqueueAsync(job1);
            await queue.EnqueueAsync(job2);
            await queue.EnqueueAsync(job3);

            // Assert FIFO order
            var out1 = await queue.DequeueAsync();
            Assert.Equal(job1, out1);

            var out2 = await queue.DequeueAsync();
            Assert.Equal(job2, out2);

            var out3 = await queue.DequeueAsync();
            Assert.Equal(job3, out3);

            // Now empty
            var out4 = await queue.DequeueAsync();
            Assert.Null(out4);
        }

        [Fact]
        public async Task DequeueAsync_OnEmptyQueue_ReturnsNull()
        {
            // Arrange
            var queue = new InMemorySimulationQueue();

            // Act
            var job = await queue.DequeueAsync();

            // Assert
            Assert.Null(job);
        }
    }
}
