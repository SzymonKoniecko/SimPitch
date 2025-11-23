using System;

namespace SimulationService.Domain.Entities;

public class MatchRound
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public bool IsDraw { get; set; }
    public bool IsPlayed { get; set; }
    public MatchRound Clone()
    {
        return new MatchRound
        {
            Id = this.Id,
            RoundId = this.RoundId,
            HomeTeamId = this.HomeTeamId,
            AwayTeamId = this.AwayTeamId,
            HomeGoals = this.HomeGoals,
            AwayGoals = this.AwayGoals,
            IsDraw = this.IsDraw,
            IsPlayed = this.IsPlayed
        };
    }
    /// <summary>
    /// SetCustomStartToSimulate method
    /// </summary>
    public MatchRound SetAsNotPlayed()
    {
        this.AwayGoals = 0;
        this.HomeGoals = 0;
        this.IsDraw = false;
        this.IsPlayed = false;

        return this;
    }
}
