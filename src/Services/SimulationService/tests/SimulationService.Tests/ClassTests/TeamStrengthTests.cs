using System;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;
using Xunit;

namespace SimulationService.Tests.Domain.ValueObjects
{
    public class TeamStrengthTests
    {
        [Fact]
        public void Create_ShouldInitializeSeasonStats()
        {
            var teamId = Guid.NewGuid();
            var leagueId = Guid.NewGuid();
            var seasonEnum = SeasonEnum.Season2023_2024;

            var teamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId);

            Assert.Equal(teamId, teamStrength.TeamId);
            Assert.NotNull(teamStrength.SeasonStats);
            Assert.Equal(teamId, teamStrength.SeasonStats.TeamId);
            Assert.Equal(seasonEnum, teamStrength.SeasonStats.SeasonYear);
            Assert.Equal(leagueId, teamStrength.SeasonStats.LeagueId);
            Assert.Equal(0, teamStrength.SeasonStats.MatchesPlayed);
        }

        [Fact]
        public void WithLikelihood_ShouldCalculateCorrectValues()
        {
            var teamId = Guid.NewGuid();
            var leagueId = Guid.NewGuid();
            var seasonEnum = SeasonEnum.Season2023_2024;

            var seasonStats = SeasonStats.CreateNew(teamId, seasonEnum, leagueId) with
            {
                MatchesPlayed = 2,
                GoalsFor = 4,
                GoalsAgainst = 1
            };

            var teamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId) with { SeasonStats = seasonStats };
            var updated = teamStrength.WithLikelihood();

            Assert.Equal(2f, updated.Likelihood.Offensive);
            Assert.Equal(0.5f, updated.Likelihood.Defensive);
        }

        [Fact]
        public void WithPosterior_ShouldCalculateCorrectValues()
        {
            var teamId = Guid.NewGuid();
            var leagueId = Guid.NewGuid();
            var seasonEnum = SeasonEnum.Season2023_2024;

            var seasonStats = SeasonStats.CreateNew(teamId, seasonEnum, leagueId) with
            {
                MatchesPlayed = 2,
                GoalsFor = 4,
                GoalsAgainst = 2
            };

            var teamStrength = TeamStrength.Create(teamId, seasonEnum, leagueId) with { SeasonStats = seasonStats };
            var updated = teamStrength.WithPosterior(1.5f);

            Assert.True(updated.Posterior.Offensive > 0);
            Assert.True(updated.Posterior.Defensive > 0);
        }

        [Fact]
        public void WithExpectedGoals_ShouldSetExpectedGoals()
        {
            var teamStrength = TeamStrength.Create(Guid.NewGuid(), SeasonEnum.Season2023_2024, Guid.NewGuid());
            var updated = teamStrength.WithExpectedGoals(2.5f);

            Assert.Equal(2.5f, updated.ExpectedGoals);
        }
    }
}
