using System;
using SimulationService.Domain.Background;
using SimulationService.Domain.ValueObjects;
using SimulationService.Domain.Enums;
using Xunit;
using SimulationService.Domain.Entities;

namespace SimulationService.Tests.Features.Background;

public class SimulationJobTests
{
    [Fact]
    public void SimulationJob_RecordEquality_WorksAsExpected()
    {
        // Arrange
        var id = Guid.NewGuid();
        var @params = new SimulationParams
        {
            SeasonYears = new() { "2023/2024" },
            LeagueId = Guid.Empty,
            Iterations = 1,
            LeagueRoundId = Guid.Empty
        };

        var state = new SimulationState(id, 0, 0f, SimulationStatus.Pending, DateTime.UtcNow);

        // Act
        var job1 = new SimulationJob(id, @params, state);
        var job2 = new SimulationJob(id, @params, state);

        // Assert
        Assert.Equal(job1, job2);
        Assert.True(job1 == job2);
        Assert.Equal(id, job1.SimulationId);
        Assert.Same(@params, job1.Params);
        Assert.Same(state, job1.State);
    }
}
