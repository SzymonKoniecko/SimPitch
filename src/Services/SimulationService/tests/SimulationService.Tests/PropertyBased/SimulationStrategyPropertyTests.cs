using FsCheck;
using FsCheck.Xunit;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Services;
using SimulationService.Domain.Services.SimulationModels;

namespace SimulationService.Tests.PropertyBased;

public class SimulationStrategyPropertyTests
{
    private static readonly SimulationModelType[] ModelTypes = Enum.GetValues<SimulationModelType>();

    /// <summary>Upper bound of goals in the Dixon-Coles probability matrix.</summary>
    private const int DixonColesMaxGoals = 10;

    private static SimulationModelType PickModel(NonNegativeInt index) => ModelTypes[index.Get % ModelTypes.Length];

    private static (int HomeGoals, int AwayGoals) Simulate(int seed, SimulationModelType modelType, int rngSeed)
    {
        var f = DomainFakers.Create(seed);
        var simulationParams = DomainFakers.SimulationParams(f);
        float leagueStrength = f.Random.Float(2.0f, 3.2f);
        var home = DomainFakers.TeamStrength(f, f.Random.Guid(), leagueStrength, simulationParams);
        var away = DomainFakers.TeamStrength(f, f.Random.Guid(), leagueStrength, simulationParams);

        return StrategyFactory.Create(modelType)
            .SimulateMatch(home, away, leagueStrength, simulationParams, new Random(rngSeed));
    }

    [Property]
    public bool SimulateMatch_NeverReturnsNegativeGoals(int seed, NonNegativeInt model, int rngSeed)
    {
        var (homeGoals, awayGoals) = Simulate(seed, PickModel(model), rngSeed);

        return homeGoals >= 0 && awayGoals >= 0;
    }

    [Property]
    public bool SimulateMatch_IsDeterministic_ForTheSameSeed(int seed, NonNegativeInt model, int rngSeed)
    {
        var modelType = PickModel(model);

        return Simulate(seed, modelType, rngSeed) == Simulate(seed, modelType, rngSeed);
    }

    [Property]
    public bool DixonColesBasedModels_StayWithinProbabilityMatrix(int seed, bool advanced, int rngSeed)
    {
        var modelType = advanced ? SimulationModelType.Advanced : SimulationModelType.DixonColes;

        var (homeGoals, awayGoals) = Simulate(seed, modelType, rngSeed);

        return homeGoals < DixonColesMaxGoals && awayGoals < DixonColesMaxGoals;
    }

    [Theory]
    [InlineData(SimulationModelType.StandardPoisson, typeof(StandardPoissonStrategy))]
    [InlineData(SimulationModelType.DixonColes, typeof(DixonColesStrategy))]
    [InlineData(SimulationModelType.BivariatePoisson, typeof(BivariatePoissonStrategy))]
    [InlineData(SimulationModelType.Advanced, typeof(AdvancedSimulationStrategy))]
    [InlineData(SimulationModelType.XgBoost, typeof(StandardPoissonStrategy))] // ML model is not simulated locally
    public void StrategyFactory_CreatesStrategyForModel(SimulationModelType modelType, Type expected)
    {
        Assert.IsType(expected, StrategyFactory.Create(modelType));
    }
}
