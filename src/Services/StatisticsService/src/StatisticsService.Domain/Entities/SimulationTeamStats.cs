using System;

namespace StatisticsService.Domain.Entities;

public class SimulationTeamStats
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public Guid TeamId { get; set; }

    public float[] PositionProbbility { get; set; }
    public float AverangePoints { get; private set; }
    public float AverangeWins { get; private set; }
    public float AverangeLosses { get; private set; }
    public float AverangeDraws { get; private set; }
    public float AverangeGoalsFor { get; private set; }
    public float AverangeGoalsAgainst { get; private set; }

    private int _simulationCount;

    public SimulationTeamStats(
        Guid id,
        Guid simulationId,
        Guid teamId,
        float[] positionProbbility,
        float averangePoints,
        float averangeWins,
        float averangeLosses,
        float averangeDraws,
        float averangeGoalsFor,
        float averangeGoalsAgainst)
    {
        Id = id;
        SimulationId = simulationId;
        TeamId = teamId;
        PositionProbbility = positionProbbility ?? Array.Empty<float>();
        AverangePoints = averangePoints;
        AverangeWins = averangeWins;
        AverangeLosses = averangeLosses;
        AverangeDraws = averangeDraws;
        AverangeGoalsFor = averangeGoalsFor;
        AverangeGoalsAgainst = averangeGoalsAgainst;
    }

    public SimulationTeamStats(Guid simulationId, Guid teamId, int positionsCount)
    {
        Id = Guid.NewGuid();
        SimulationId = simulationId;
        TeamId = teamId;
        PositionProbbility = new float[positionsCount];
        _simulationCount = 0;
    }

    public void AddFromScoreboardStats(
        int positionIndex,
        int points,
        int wins,
        int losses,
        int draws,
        int goalsFor,
        int goalsAgainst)
    {
        if (positionIndex < 0 || positionIndex >= PositionProbbility.Length)
            throw new ArgumentOutOfRangeException(nameof(positionIndex));

        _simulationCount++;

        PositionProbbility[positionIndex] += 1f;

        AverangePoints = UpdateAverage(AverangePoints, points);
        AverangeWins = UpdateAverage(AverangeWins, wins);
        AverangeLosses = UpdateAverage(AverangeLosses, losses);
        AverangeDraws = UpdateAverage(AverangeDraws, draws);
        AverangeGoalsFor = UpdateAverage(AverangeGoalsFor, goalsFor);
        AverangeGoalsAgainst = UpdateAverage(AverangeGoalsAgainst, goalsAgainst);
    }

    public float[] SetNormalizedPositionProbability()
    {
        if (_simulationCount == 0)
        {
            PositionProbbility = new float[PositionProbbility.Length];
            return PositionProbbility;
        }

        for (int i = 0; i < PositionProbbility.Length; i++)
            PositionProbbility[i] /= _simulationCount;

        return PositionProbbility;
    }

    private float UpdateAverage(float currentAverage, float newValue)
    {
        return ((currentAverage * (_simulationCount - 1)) + newValue) / _simulationCount;
    }
}