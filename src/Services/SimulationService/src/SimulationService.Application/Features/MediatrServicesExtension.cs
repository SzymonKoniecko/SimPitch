using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultById;
using SimulationService.Application.Features.Simulations.Commands.SetSimulation;
using SimulationService.Application.Features.Scoreboards.Commands;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationIdsByDate;
using SimulationService.Application.Features.Predict.Commands;
using SimulationService.Application.Features.Predict.Commands.StartPredictionCommand;
using SimulationService.Application.Features.Predict.Commands.SyncPredictionIterationResultCommand;
using FluentValidation;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.SeasonsStats.DTOs;
namespace SimulationService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(InitSimulationContentCommand).Assembly);
        services.AddMediatR(typeof(RunSimulationCommandHandler).Assembly);
        services.AddMediatR(typeof(SetSimulationCommand).Assembly);
        services.AddMediatR(typeof(CreateIterationResultCommand).Assembly);
        services.AddMediatR(typeof(CreateScoreboardByIterationResultCommand).Assembly);
        services.AddMediatR(typeof(StartPredictionCommand).Assembly);
        services.AddMediatR(typeof(SyncPredictionIterationResultCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(InitSimulationContentCommandHandler).Assembly);
        services.AddMediatR(typeof(RunSimulationCommandHandler).Assembly);
        services.AddMediatR(typeof(SetSimulationCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateIterationResultCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateScoreboardByIterationResultCommandHandler).Assembly);
        services.AddMediatR(typeof(StartPredictionCommandHandler).Assembly);
        services.AddMediatR(typeof(SyncPredictionIterationResultCommandHandler).Assembly);

        // Queries
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcQuery).Assembly);
        services.AddMediatR(typeof(GetLeagueByIdQuery).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByIdQuery).Assembly);
        services.AddMediatR(typeof(GetIterationResultByIdQuery).Assembly);
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQuery).Assembly);
        services.AddMediatR(typeof(GetSimulationOverviewByIdQuery).Assembly);
        services.AddMediatR(typeof(GetAllSimulationOverviewsQuery).Assembly);
        services.AddMediatR(typeof(GetSimulationIdsByDateQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcHandler).Assembly);
        services.AddMediatR(typeof(GetLeagueByIdHandler).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByIdHandler).Assembly);
        services.AddMediatR(typeof(GetIterationResultByIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetSimulationOverviewByIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetAllSimulationOverviewsQueryHandler).Assembly);
        services.AddMediatR(typeof(GetSimulationIdsByDateQueryHandler).Assembly);

        // Validators
        services.AddTransient<RunSimulationCommandValidator>();
        services.AddTransient<IValidator<SyncPredictionIterationResultCommand>, SyncPredictionIterationResultCommandValidator>();
        services.AddTransient<IValidator<IterationResultDto>, IterationResultDtoValidator>();
        services.AddTransient<IValidator<StrengthItemDto>, StrengthItemDtoValidator>();
        services.AddTransient<IValidator<TeamStrengthDto>, TeamStrengthDtoValidator>();
        services.AddTransient<IValidator<MatchRoundDto>, MatchRoundDtoValidator>();
        services.AddTransient<IValidator<SeasonStatsDto>, SeasonStatsDtoValidator>();

        return services;
    }
}
