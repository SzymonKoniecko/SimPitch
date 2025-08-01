namespace LoggingService.Domain.Entities;
public class LogEntry
{
    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public string Level { get; set; }
    public string StackTrace { get; set; }
    public string Source { get; set; }
    public string Context { get; set; }
}