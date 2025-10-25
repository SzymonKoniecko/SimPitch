using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public class SimulationState
{
    public SimulationStatus SimulationStatus { get; set; }
    public float Progress { get; set; }
    public DateTime UpdatedAt { get; set; }

    public SimulationState(SimulationStatus simulationStatus, float progress, DateTime dateTime)
    {
        SimulationStatus = simulationStatus;
        Progress = progress;
        UpdatedAt = dateTime;
    }

    public SimulationState Start()
    {
        this.SimulationStatus = SimulationStatus.Running;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState Update(float progress)
    {
        this.SimulationStatus = SimulationStatus.Running;
        this.Progress = progress;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState SetFailed()
    {
        this.SimulationStatus = SimulationStatus.Failed;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState SetCancelled()
    {
        this.SimulationStatus = SimulationStatus.Cancelled;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState SetCompleted()
    {
        this.SimulationStatus = SimulationStatus.Completed;
        this.UpdatedAt = DateTime.Now;
        return this;
    }
}