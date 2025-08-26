using System;
using FluentValidation;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.DomainValidators;

public class SimulationContentValidator : AbstractValidator<SimulationContent>
{
    public SimulationContentValidator()
    {
        RuleFor(x => x.PriorLeagueStrength)
            .NotEmpty()
            .WithMessage("Simulation must contain PriorLeagueStrength.");
            
        RuleForEach(x => x.TeamsStrengthDictionary)
            .ChildRules(team =>
            {
                team.RuleFor(m => m.Value)
                    .NotEmpty()
                    .WithMessage("TeamsStrengthDictionary must contain value in dictionary!");
            });
    }
}
