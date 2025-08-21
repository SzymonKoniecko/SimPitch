using System;
using FluentValidation;
using SportsDataService.Application.Features.League.Queries.GetLeagueById;

namespace SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

public class GetAllLeagueRoundsByParamsValidator : AbstractValidator<GetAllLeagueRoundsByParamsQuery>
{
    public GetAllLeagueRoundsByParamsValidator()
    {
        RuleFor(x => x.leagueRoundFilterDto.SeasonYear)
            .NotEmpty().WithMessage("SeasonYear is required.")
             .Matches(@"^\d{4}/\d{4}$").WithMessage("SeasonYear must be in format YYYY/YYYY");

        RuleFor(x => x.leagueRoundFilterDto.Round)
                .Must(round => round > 0)
                .WithMessage("If provided, Round must be greater than zero.");
    }
}
