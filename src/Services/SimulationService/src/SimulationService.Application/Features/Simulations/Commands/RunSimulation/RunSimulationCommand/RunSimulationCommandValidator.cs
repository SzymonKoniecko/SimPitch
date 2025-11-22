using System;
using FluentValidation;

namespace SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;

public class RunSimulationCommandValidator : AbstractValidator<RunSimulationCommand>
{
    public RunSimulationCommandValidator()
    {
        RuleFor(x => x.SimulationParamsDto.SeasonYears)
            .NotEmpty()
            .WithMessage("SeasonYear list is required.");
        RuleForEach(x => x.SimulationParamsDto.SeasonYears)
            .Must(season => season.Length > 4 && season[4] == '/')
            .WithMessage("SeasonYear has missing '/'.");

        RuleFor(x => x.SimulationParamsDto.Iterations)
            .Must(it => it > 0)
            .WithMessage("Number of iterations for simulations should be highter than 0 !");

        RuleFor(x => x.SimulationParamsDto.LeagueId)
            .Must(league => league != null)
            .WithMessage("If provided, LeagueId must be filled in!");
    }
}
