using Microsoft.Extensions.Logging;

namespace BlankSlate.Functions;

// LoggerMessage source-generated helpers for better perf and analyzer compliance
internal static partial class McpToolsLogs
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Serving C# best practices via MCP tool {ToolName}")]
    public static partial void ServingBestPractices(this ILogger logger, string toolName);

    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "Loading best practices from {FilePath}")]
    public static partial void LoadingBestPractices(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 3, Level = LogLevel.Debug, Message = "Serving cached best practices content from {FilePath}")]
    public static partial void ServingCachedBestPractices(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "Best practices file not found at {FilePath}; serving fallback")]
    public static partial void BestPracticesFileNotFound(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "Failed to load best practices content; serving fallback")]
    public static partial void FailedToLoadBestPractices(this ILogger logger, Exception exception);
}