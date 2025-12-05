using System;
using SportsDataService.Domain.Interfaces.Read;
using FluentValidation;

namespace SportsDataService.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator(
        ICountryReadRepository countryReadRepository,
        ILeagueReadRepository leagueReadRepository,
        IStadiumReadRepository stadiumReadRepository
    )
    {
        RuleFor(command => command.Team.CountryId)
            .NotNull()
            .WithMessage("Country ID cannot be null")
            .MustAsync(async (countryId, cancellationToken) =>
                await countryReadRepository.CountryExistsAsync(countryId, cancellationToken))
            .WithMessage("Country with the specified ID does not exist");
        RuleFor(command => command.Team.StadiumId)
            .NotNull()
            .WithMessage("Stadium ID cannot be null")
            .MustAsync(async (stadiumId, cancellationToken) =>
                await stadiumReadRepository.StadiumExistsAsync(stadiumId, cancellationToken))
            .WithMessage("Stadium with the specified ID does not exist");
    }
}