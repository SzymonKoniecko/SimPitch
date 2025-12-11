using System;
using SimulationService.Domain.Consts;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;
using Xunit;

namespace SimulationService.Tests.Domain.ValueObjects
{
    public class TeamStrengthTests
    {
        public SimulationParams SimulationParams { get; set; }
        public TeamStrengthTests()
        {
            SimulationParams = new()
            {
                Seed = 1000,
                GamesToReachTrust = SimulationConsts.GAMES_TO_REACH_TRUST,
                ConfidenceLevel = SimulationConsts.SIMULATION_CONFIDENCE_LEVEL,
                HomeAdvantage = SimulationConsts.HOME_ADVANTAGE,
                NoiseFactor = SimulationConsts.NOISE_FACTOR
            };
        }
        [Fact]
        public void Create_ShouldInitializeSeasonStats()
        {
            var teamId = Guid.NewGuid();
            var leagueId = Guid.NewGuid();
            var seasonEnum = SeasonEnum.Season2023_2024;
            float leagueStrength = 0.75f;

            var teamStrength = TeamStrength.Create(Guid.NewGuid(), teamId, seasonEnum, leagueId, leagueStrength);

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
            float leagueStrength = 0.75f;

            var seasonStats = SeasonStats.CreateNew(Guid.NewGuid(), teamId, seasonEnum, leagueId, leagueStrength) with
            {
                MatchesPlayed = 2,
                GoalsFor = 4,
                GoalsAgainst = 1
            };

            var teamStrength = TeamStrength.Create(Guid.NewGuid(), teamId, seasonEnum, leagueId, leagueStrength) with { SeasonStats = seasonStats };
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
            float leagueStrength = 0.75f;

            var seasonStats = SeasonStats.CreateNew(Guid.NewGuid(), teamId, seasonEnum, leagueId, leagueStrength) with
            {
                MatchesPlayed = 2,
                GoalsFor = 4,
                GoalsAgainst = 2
            };

            var teamStrength = TeamStrength.Create(Guid.NewGuid(), teamId, seasonEnum, leagueId, leagueStrength) with { SeasonStats = seasonStats };
            var updated = teamStrength.WithPosterior(1.5f, SimulationParams);

            Assert.True(updated.Posterior.Offensive > 0);
            Assert.True(updated.Posterior.Defensive > 0);
        }

        [Fact]
        public void WithExpectedGoals_ShouldSetExpectedGoals()
        {
            float leagueStrength = 0.75f;
            var teamStrength = TeamStrength.Create(Guid.NewGuid(), Guid.NewGuid(), SeasonEnum.Season2023_2024, Guid.NewGuid(), leagueStrength);
            var updated = teamStrength.WithExpectedGoals(2.5f);

            Assert.Equal(2.5f, updated.ExpectedGoals);
        }

        [Fact]
        public void Merge_ShouldThrow_WhenDifferentTeams()
        {
            var season1 = SeasonStats.CreateNew(Guid.NewGuid(), Guid.NewGuid(), SeasonEnum.Season2023_2024, Guid.NewGuid(), 1.0f);
            var season2 = SeasonStats.CreateNew(Guid.NewGuid(), Guid.NewGuid(), SeasonEnum.Season2023_2024, Guid.NewGuid(), 1.0f);

            Assert.Throws<InvalidOperationException>(() => season1.Merge(season1, season2));
        }

        [Fact]
        public void Merge_ShouldCombineMatchesAndGoals_WithSameStrength()
        {
            var teamId = Guid.NewGuid();
            var leagueId = Guid.NewGuid();

            var season1 = SeasonStats.CreateNew(Guid.NewGuid(), teamId, SeasonEnum.Season2023_2024, leagueId, 1.0f) with
            {
                MatchesPlayed = 2,
                Wins = 1,
                Losses = 1,
                GoalsFor = 3,
                GoalsAgainst = 2
            };

            var season2 = SeasonStats.CreateNew(Guid.NewGuid(), teamId, SeasonEnum.Season2023_2024, leagueId, 1.0f) with
            {
                MatchesPlayed = 1,
                Wins = 1,
                Losses = 0,
                GoalsFor = 2,
                GoalsAgainst = 1
            };

            var merged = season1.Merge(season1, season2);

            Assert.Equal(3, merged.MatchesPlayed);
            Assert.Equal(2, merged.Wins);
            Assert.Equal(1, merged.Losses);
            Assert.Equal(5, merged.GoalsFor);
            Assert.Equal(3, merged.GoalsAgainst);
        }
    }
}
