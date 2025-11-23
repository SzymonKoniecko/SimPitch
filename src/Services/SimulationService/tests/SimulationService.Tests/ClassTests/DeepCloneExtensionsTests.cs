using System;
using SimulationService.Application.Extensions;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.ClassTests;

public class DeepCloneExtensionsTests
{
    [Fact]
    public void CloneSimulationData_ShouldCreateIndependentCopy_OfMatchRounds()
    {
        // Arrange: Przygotuj oryginalne dane
        var originalMatch = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = Guid.NewGuid(),
            AwayTeamId = Guid.NewGuid(),
            HomeGoals = 2,
            AwayGoals = 1,
            IsPlayed = false,
            IsDraw = false
        };

        var originalMatchRounds = new List<MatchRound> { originalMatch };
        var originalTeamStrengths = new Dictionary<Guid, List<TeamStrength>>();

        // Act: Sklonuj dane
        var (clonedMatches, _) = DeepCloneExtensions.CloneSimulationDataManual(
            originalMatchRounds,
            originalTeamStrengths
        );

        // Assert: Sprawdź czy kopia ma takie same wartości
        Assert.Single(clonedMatches);
        Assert.Equal(originalMatch.Id, clonedMatches[0].Id);
        Assert.Equal(2, clonedMatches[0].HomeGoals);
        Assert.Equal(1, clonedMatches[0].AwayGoals);
        Assert.False(clonedMatches[0].IsPlayed);

        // KLUCZOWY TEST: Modyfikuj klonowaną kopię
        clonedMatches[0].HomeGoals = 5;
        clonedMatches[0].AwayGoals = 3;
        clonedMatches[0].IsPlayed = true;

        // Sprawdź że oryginał NIE zmienił się
        Assert.Equal(2, originalMatch.HomeGoals); // ✅ Powinno być 2, nie 5
        Assert.Equal(1, originalMatch.AwayGoals); // ✅ Powinno być 1, nie 3
        Assert.False(originalMatch.IsPlayed);      // ✅ Powinno być false
    }
    
    [Fact]
    public void CloneSimulationData_ShouldNotShareReferences_WhenModifyingCollections()
    {
        // Arrange: Przygotuj dane z wieloma meczami i drużynami
        var match1 = new MatchRound { Id = Guid.NewGuid(), HomeGoals = 1, IsPlayed = false };
        var match2 = new MatchRound { Id = Guid.NewGuid(), HomeGoals = 2, IsPlayed = false };

        var originalMatches = new List<MatchRound> { match1, match2 };
        var originalTeams = new Dictionary<Guid, List<TeamStrength>>();

        // Act: Sklonuj
        var (clonedMatches, _) = DeepCloneExtensions.CloneSimulationDataManual(
            originalMatches,
            originalTeams
        );

        // Assert: Modyfikuj kolekcję klonowaną (dodaj, usuń)
        clonedMatches.Add(new MatchRound { Id = Guid.NewGuid(), HomeGoals = 3, IsPlayed = false });
        clonedMatches.RemoveAt(0);

        // Oryginał powinien mieć nadal 2 elementy
        Assert.Equal(2, originalMatches.Count);

        // Klonowana kopia powinna mieć 2 elementy (2 - 1 usunięty + 1 dodany)
        Assert.Equal(2, clonedMatches.Count);

        // Sprawdź że elementy w kolekcjach są różne
        Assert.NotEqual(originalMatches[0].Id, clonedMatches[0].Id);
    }

    [Fact]
    public void CloneSimulationData_ShouldHandleComplexScenario_WithMultipleIterations()
    {
        // Arrange: Symuluj scenariusz z pętli (2 iteracje)
        var teamId1 = Guid.NewGuid();
        var teamId2 = Guid.NewGuid();
        var leagueId = Guid.NewGuid();

        var match = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = teamId1,
            AwayTeamId = teamId2,
            HomeGoals = 0,
            AwayGoals = 0,
            IsPlayed = false
        };

        var team1 = TeamStrength.Create(teamId1, SeasonEnum.Season2023_2024, leagueId, 2.5f);
        var team2 = TeamStrength.Create(teamId2, SeasonEnum.Season2023_2024, leagueId, 2.5f);

        var originalMatches = new List<MatchRound> { match };
        var originalTeams = new Dictionary<Guid, List<TeamStrength>>
            {
                { teamId1, new List<TeamStrength> { team1 } },
                { teamId2, new List<TeamStrength> { team2 } }
            };

        // Act & Assert: Symuluj 2 iteracje
        for (int iteration = 1; iteration <= 2; iteration++)
        {
            // Sklonuj dane na początek każdej iteracji
            var (clonedMatches, clonedTeams) = DeepCloneExtensions.CloneSimulationDataManual(
                originalMatches,
                originalTeams
            );

            // Symuluj: Zmień dane w klonowanej kopii
            clonedMatches[0].HomeGoals = iteration * 10;
            clonedMatches[0].AwayGoals = iteration * 5;
            clonedMatches[0].IsPlayed = true;

            // Oryginalne dane powinny pozostać niezmienione
            Assert.Equal(0, originalMatches[0].HomeGoals);
            Assert.Equal(0, originalMatches[0].AwayGoals);
            Assert.False(originalMatches[0].IsPlayed);

            // W każdej iteracji kopia powinna mieć inne wartości
            if (iteration == 1)
            {
                Assert.Equal(10, clonedMatches[0].HomeGoals);
                Assert.Equal(5, clonedMatches[0].AwayGoals);
            }
            else if (iteration == 2)
            {
                Assert.Equal(20, clonedMatches[0].HomeGoals);
                Assert.Equal(10, clonedMatches[0].AwayGoals);
            }
        }
    }
}