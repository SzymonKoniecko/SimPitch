using System;
using LoggingService.SimPitchProtos;

namespace StatisticsService.Infrastructure.Logging;

public class GrpcLoggerOptions
{
    public const string SectionName = "GrpcLogging";

    public string Address { get; set; } = string.Empty;
    public string SourceName { get; set; } = "StatisticsService";
    public LogService.LogServiceClient GrpcClient { get; set; } = null!;
}
