using System;
using LoggingService.SimPitchProtos;

namespace SimulationService.Infrastructure.Logging;

public class GrpcLoggerOptions
{
    public const string SectionName = "GrpcLogging";

    public string Address { get; set; } = string.Empty;
    public string SourceName { get; set; } = "SimulationService";
    public LogService.LogServiceClient GrpcClient { get; set; } = null!;
}
