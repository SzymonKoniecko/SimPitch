using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.Features.Simulations;

public class SeasonStatsTests
{
    [Fact]
    public void Merge_ShouldSumStats_AndTakeNewLeagueStrength()
    {
        // Arrange: Scenariusz "Awans do silniejszej ligi"
        var teamId = Guid.NewGuid();
        var oldLeagueId = Guid.NewGuid();
        var newLeagueId = Guid.NewGuid();

        // Historia (Słabsza liga, 2023/24)
        var accumulator = new SeasonStats(teamId, SeasonEnum.Season2023_2024, oldLeagueId, 2.0f,
            matchesPlayed: 10, wins: 5, losses: 3, draws: 2, goalsFor: 20, goalsAgainst: 10);

        // Nowe dane (Silniejsza liga, 2024/25)
        var newData = new SeasonStats(teamId, SeasonEnum.Season2024_2025, newLeagueId, 3.0f,
            matchesPlayed: 5, wins: 1, losses: 2, draws: 2, goalsFor: 5, goalsAgainst: 8);

        // Act
        var result = accumulator.Merge(accumulator, newData);

        // Assert
        // 1. Sprawdź sumowanie (Czysta matematyka)
        Assert.Equal(15, result.MatchesPlayed); // 10 + 5
        Assert.Equal(6, result.Wins); // 5 + 1
        Assert.Equal(25, result.GoalsFor); // 20 + 5 (BEZ SKALOWANIA!)

        // 2. Sprawdź kontekst (Musi wziąć z newData)
        Assert.Equal(3.0f, result.LeagueStrength); // Nowa siła
        Assert.Equal(SeasonEnum.Season2024_2025, result.SeasonYear); // Nowy rok
        Assert.Equal(newLeagueId, result.LeagueId); // Nowa liga
    }

    [Fact]
    public void Merge_ShouldThrowException_WhenTeamIdsMismatch()
    {
        // Arrange
        var stats1 = SeasonStats.CreateNew(Guid.NewGuid(), SeasonEnum.Season2023_2024, Guid.NewGuid(), 2.5f);
        var stats2 = SeasonStats.CreateNew(Guid.NewGuid(), SeasonEnum.Season2023_2024, Guid.NewGuid(), 2.5f);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => stats1.Merge(stats1, stats2));
    }

    [Fact]
    public void Increment_ShouldUpdateStatsCorrectly_ForHomeWin()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var stats = SeasonStats.CreateNew(teamId, SeasonEnum.Season2023_2024, Guid.NewGuid(), 2.5f);
        var match = new MatchRound
        {
            HomeTeamId = teamId,
            HomeGoals = 3,
            AwayGoals = 1
        };

        // Act
        var updated = stats.Increment(match, isHomeTeam: true);

        // Assert
        Assert.Equal(1, updated.MatchesPlayed);
        Assert.Equal(1, updated.Wins);
        Assert.Equal(3, updated.GoalsFor);
        Assert.Equal(1, updated.GoalsAgainst);
    }
}