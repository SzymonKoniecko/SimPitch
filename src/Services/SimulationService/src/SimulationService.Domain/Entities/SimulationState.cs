using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.Entities;

public class SimulationState
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SimulationId { get; set; }

    public int LastCompletedIteration { get; set; }

    public float ProgressPercent { get; set; }

    public SimulationStatus State { get; set; }

    public DateTime UpdatedAt { get; set; }

    public SimulationState()
    {
        
    }
    public SimulationState(Guid simulationId, int lastCompletedIteration, float progress, SimulationStatus simulationStatus, DateTime dateTime)
    {
        SimulationId = simulationId;
        LastCompletedIteration = lastCompletedIteration;
        ProgressPercent = progress;
        State = simulationStatus;
        UpdatedAt = dateTime;
    }

    public SimulationState Update(float progress)
    {
        this.State = SimulationStatus.Running;
        this.ProgressPercent = progress;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState SetFailed()
    {
        this.State = SimulationStatus.Failed;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState SetCancelled()
    {
        this.State = SimulationStatus.Cancelled;
        this.UpdatedAt = DateTime.Now;
        return this;
    }

    public SimulationState SetCompleted()
    {
        this.State = SimulationStatus.Completed;
        this.UpdatedAt = DateTime.Now;
        return this;
    }
}