using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SportsDataService.Application.Features.Country.Queries.GetAllCountries;
using SportsDataService.Application.Features.Country.Queries.GetCountryById;
using SportsDataService.Application.Features.League.Queries.GetAllLeagues;
using SportsDataService.Application.Features.League.Queries.GetLeagueById;
using SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;
using SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;
using SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonStatsById;
using SportsDataService.Application.Features.Stadium.Commands.CreateStadium;
using SportsDataService.Application.Features.Stadium.Queries.GetAllStadiums;
using SportsDataService.Application.Features.Stadium.Queries.GetStadiumById;
using SportsDataService.Application.Features.Teams.Commands.CreateTeam;
using SportsDataService.Application.Features.Teams.Queries.GetAllTeams;
using SportsDataService.Application.Features.Teams.Queries.GetTeamById;

namespace SportsDataService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(CreateTeamCommand).Assembly);
        services.AddMediatR(typeof(CreateStadiumCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(CreateTeamCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateStadiumCommandHandler).Assembly);

        // Queries
        services.AddMediatR(typeof(GetAllCountriesQuery).Assembly);
        services.AddMediatR(typeof(GetCountryByIdQuery).Assembly);
        services.AddMediatR(typeof(GetSeasonStatsByIdQuery).Assembly);
        services.AddMediatR(typeof(GetAllLeaguesQuery).Assembly);
        services.AddMediatR(typeof(GetLeagueByIdQuery).Assembly);
        services.AddMediatR(typeof(GetAllStadiumsQuery).Assembly);
        services.AddMediatR(typeof(GetStadiumByIdQuery).Assembly);
        services.AddMediatR(typeof(GetAllTeamsQuery).Assembly);
        services.AddMediatR(typeof(GetTeamByIdQuery).Assembly);
        services.AddMediatR(typeof(GetAllLeagueRoundsByParamsQuery).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByRoundIdQuery).Assembly);


        // Query Handlers
        services.AddMediatR(typeof(GetAllCountriesHandler).Assembly);
        services.AddMediatR(typeof(GetCountryByIdHandler).Assembly);
        services.AddMediatR(typeof(GetSeasonStatsByIdHandler).Assembly);
        services.AddMediatR(typeof(GetAllLeaguesHandler).Assembly);
        services.AddMediatR(typeof(GetLeagueByIdHandler).Assembly);
        services.AddMediatR(typeof(GetAllStadiumsHandler).Assembly);
        services.AddMediatR(typeof(GetStadiumByIdHandler).Assembly);
        services.AddMediatR(typeof(GetAllTeamsHandler).Assembly);
        services.AddMediatR(typeof(GetTeamByIdHandler).Assembly);
        services.AddMediatR(typeof(GetAllLeagueRoundsByParamsHandler).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByRoundIdHandler).Assembly);

        // Validators
        services.AddTransient<CreateTeamCommandValidator>();
        services.AddTransient<GetAllLeagueRoundsByParamsValidator>();

        return services;
    }
}
