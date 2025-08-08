using System;

namespace SportsDataService.Infrastructure.Logging;
public class GrpcLoggerOptions
{
    public const string SectionName = "GrpcLogging";
    
    public string Address { get; set; } = string.Empty;
    public string SourceName { get; set; } = "SportsDataService";
}
