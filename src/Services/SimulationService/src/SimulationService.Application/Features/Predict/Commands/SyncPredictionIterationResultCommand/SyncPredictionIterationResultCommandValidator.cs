using System;
using System.Data;
using FluentValidation;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.SeasonsStats.DTOs;

namespace SimulationService.Application.Features.Predict.Commands.SyncPredictionIterationResultCommand;

public class SyncPredictionIterationResultCommandValidator : AbstractValidator<SyncPredictionIterationResultCommand>
{
    public SyncPredictionIterationResultCommandValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.IterationResult)
            .NotNull()
            .SetValidator(new IterationResultDtoValidator());
    }
}

public sealed class IterationResultDtoValidator : AbstractValidator<IterationResultDto>
{
    public IterationResultDtoValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Id must not be empty.");

        RuleFor(x => x.SimulationId)
            .NotEqual(Guid.Empty).WithMessage("SimulationId must not be empty.");

        RuleFor(x => x.IterationIndex)
            .GreaterThanOrEqualTo(0).WithMessage("IterationIndex must be >= 0.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.");

        RuleFor(x => x.ExecutionTime)
            .Must(t => t >= TimeSpan.Zero).WithMessage("ExecutionTime must be >= 0.");

        RuleFor(x => x.TeamStrengths)
            .NotNull().WithMessage("TeamStrengths is required.")
            .NotEmpty().WithMessage("TeamStrengths must not be empty.");

        RuleForEach(x => x.TeamStrengths)
            .SetValidator(new TeamStrengthDtoValidator());

        RuleFor(x => x.SimulatedMatchRounds)
            .NotNull().WithMessage("SimulatedMatchRounds is required.")
            .NotEmpty().WithMessage("SimulatedMatchRounds must not be empty.");

        RuleForEach(x => x.SimulatedMatchRounds)
            .SetValidator(new MatchRoundDtoValidator());
    }
}
public sealed class StrengthItemDtoValidator : AbstractValidator<StrengthItemDto>
{
    public StrengthItemDtoValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.Offensive)
            .Must(v => !float.IsNaN(v) && !float.IsInfinity(v))
            .WithMessage("Offensive must be a finite number.");

        RuleFor(x => x.Defensive)
            .Must(v => !float.IsNaN(v) && !float.IsInfinity(v))
            .WithMessage("Defensive must be a finite number.");

        RuleFor(x => x.Offensive)
            .GreaterThan(0f).WithMessage("Offensive must be > 0.");

        RuleFor(x => x.Defensive)
            .GreaterThan(0f).WithMessage("Defensive must be > 0.");
    }
}

public sealed class TeamStrengthDtoValidator : AbstractValidator<TeamStrengthDto>
{
    public TeamStrengthDtoValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.TeamId)
            .NotEqual(Guid.Empty).WithMessage("TeamId must not be empty.");

        RuleFor(x => x.Likelihood)
            .NotNull().SetValidator(new StrengthItemDtoValidator());

        RuleFor(x => x.Posterior)
            .NotNull().SetValidator(new StrengthItemDtoValidator());

        RuleFor(x => x.ExpectedGoals)
            .Must(v => !float.IsNaN(v) && !float.IsInfinity(v))
            .WithMessage("ExpectedGoals must be a finite number.")
            .GreaterThanOrEqualTo(0f).WithMessage("ExpectedGoals must be >= 0.");

        RuleFor(x => x.LastUpdate)
            .NotEmpty().WithMessage("LastUpdate is required.");

        RuleFor(x => x.SeasonStats)
            .NotNull().WithMessage("SeasonStats is required.")
            .SetValidator(new SeasonStatsDtoValidator());
    }
}

public sealed class MatchRoundDtoValidator : AbstractValidator<MatchRoundDto>
{
    public MatchRoundDtoValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("MatchRound.Id must not be empty.");

        RuleFor(x => x.RoundId)
            .NotEqual(Guid.Empty).WithMessage("MatchRound.RoundId must not be empty.");

        RuleFor(x => x.HomeTeamId)
            .NotEqual(Guid.Empty).WithMessage("MatchRound.HomeTeamId must not be empty.");

        RuleFor(x => x.AwayTeamId)
            .NotEqual(Guid.Empty).WithMessage("MatchRound.AwayTeamId must not be empty.");

        RuleFor(x => x)
            .Must(x => x.HomeTeamId != x.AwayTeamId)
            .WithMessage("HomeTeamId and AwayTeamId must be different.");

        // spójność IsPlayed vs gole
        When(x => x.IsPlayed, () =>
        {
            RuleFor(x => x.HomeGoals)
                .NotNull().WithMessage("HomeGoals must be provided when IsPlayed=true.")
                .GreaterThanOrEqualTo(0).WithMessage("HomeGoals must be >= 0.");

            RuleFor(x => x.AwayGoals)
                .NotNull().WithMessage("AwayGoals must be provided when IsPlayed=true.")
                .GreaterThanOrEqualTo(0).WithMessage("AwayGoals must be >= 0.");

            RuleFor(x => x.IsDraw)
                .NotNull().WithMessage("IsDraw must be provided when IsPlayed=true.");

            RuleFor(x => x)
                .Must(x => x.IsDraw == ((x.HomeGoals ?? 0) == (x.AwayGoals ?? 0)))
                .WithMessage("IsDraw must match HomeGoals == AwayGoals when IsPlayed=true.");
        });

        When(x => !x.IsPlayed, () =>
        {
            RuleFor(x => x.HomeGoals)
                .Must(v => v is null || v >= 0).WithMessage("HomeGoals must be null or >=0 when IsPlayed=false.");
            RuleFor(x => x.AwayGoals)
                .Must(v => v is null || v >= 0).WithMessage("AwayGoals must be null or >=0 when IsPlayed=false.");
        });
    }
}
public sealed class SeasonStatsDtoValidator : AbstractValidator<SeasonStatsDto>
{
    public SeasonStatsDtoValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.TeamId)
            .NotEqual(Guid.Empty).WithMessage("SeasonStats.TeamId must not be empty.");

        RuleFor(x => x.LeagueId)
            .NotEqual(Guid.Empty).WithMessage("SeasonStats.LeagueId must not be empty.");

        RuleFor(x => x.SeasonYear)
            .IsInEnum().WithMessage("SeasonStats.SeasonYear has an invalid value.");

        RuleFor(x => x.LeagueStrength)
            .Must(v => !float.IsNaN(v) && !float.IsInfinity(v))
            .WithMessage("SeasonStats.LeagueStrength must be a finite number.")
            .GreaterThan(0f).WithMessage("SeasonStats.LeagueStrength must be > 0.");

        RuleFor(x => x.MatchesPlayed)
            .GreaterThanOrEqualTo(0).WithMessage("MatchesPlayed must be >= 0.");

        RuleFor(x => x.Wins)
            .GreaterThanOrEqualTo(0).WithMessage("Wins must be >= 0.");

        RuleFor(x => x.Draws)
            .GreaterThanOrEqualTo(0).WithMessage("Draws must be >= 0.");

        RuleFor(x => x.Losses)
            .GreaterThanOrEqualTo(0).WithMessage("Losses must be >= 0.");

        RuleFor(x => x.GoalsFor)
            .GreaterThanOrEqualTo(0).WithMessage("GoalsFor must be >= 0.");

        RuleFor(x => x.GoalsAgainst)
            .GreaterThanOrEqualTo(0).WithMessage("GoalsAgainst must be >= 0.");

        RuleFor(x => x)
            .Must(x => x.MatchesPlayed == x.Wins + x.Draws + x.Losses)
            .WithMessage("MatchesPlayed must equal Wins + Draws + Losses.");

        When(x => x.MatchesPlayed == 0, () =>
        {
            RuleFor(x => x)
                .Must(x => x.Wins == 0 && x.Draws == 0 && x.Losses == 0 && x.GoalsFor == 0 && x.GoalsAgainst == 0)
                .WithMessage("When MatchesPlayed is 0, Wins/Draws/Losses/Goals must all be 0.");
        });
    }
}