namespace LogViewer;

public record Log(DateTime Timestamp, string LogLevel, string Category, string Message);
