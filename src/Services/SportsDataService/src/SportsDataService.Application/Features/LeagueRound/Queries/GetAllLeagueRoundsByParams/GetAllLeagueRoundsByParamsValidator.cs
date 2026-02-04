using System;
using FluentValidation;
using SportsDataService.Application.Features.League.Queries.GetLeagueById;

namespace SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

public class GetAllLeagueRoundsByParamsValidator : AbstractValidator<GetAllLeagueRoundsByParamsQuery>
{
    public GetAllLeagueRoundsByParamsValidator()
    {
        RuleFor(x => x.leagueRoundFilterDto.SeasonYear)
            .Matches(@"^\d{4}/\d{4}$")
            .When(x => !string.IsNullOrEmpty(x.leagueRoundFilterDto.SeasonYear))
            .WithMessage("SeasonYear must be in format YYYY/YYYY");

        RuleFor(x => x.leagueRoundFilterDto.LeagueId)
            .Must(league => league != null)
            .WithMessage("If provided, LeagueId must be filled in!");
    }
}
