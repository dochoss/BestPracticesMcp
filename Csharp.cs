using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace BlankSlate.Functions;

/// <summary>
/// Provides tools for retrieving best practices for the C# programming language.
/// Utilizes a process-wide cache to minimize disk reads and improve performance.
/// </summary>
/// <remarks>
/// This class is intended for internal use and is designed to be used within a function app context.
/// It reads best practices from a markdown file and caches the content for a short duration.
/// </remarks>
public class McpTools(ILogger<McpTools> logger)
{
    // Simple process-wide cache to avoid disk reads on every invocation
    private static readonly SemaphoreSlim CacheLock = new(1, 1);
    private static string? _cachedContent;
    private static DateTimeOffset _cachedFileWrite;
    private static DateTimeOffset _cacheExpires;

    [Function(nameof(GetCsharpBestPractices))]
    public async Task<string> GetCsharpBestPractices(
        [McpToolTrigger("get_csharp_best_practices", "Retrieves best practices for the C# programming language")]
            ToolInvocationContext toolContext,
        CancellationToken cancellationToken)
    {
        logger.ServingBestPractices("get_csharp_best_practices");
        try
        {
            // Resolve path relative to the function app's working directory
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "csharp-best-practices.md");
            if (File.Exists(filePath))
            {
                var lastWrite = File.GetLastWriteTimeUtc(filePath);

                // Return cached if still valid and file unchanged
                if (_cachedContent is not null && _cacheExpires > DateTimeOffset.UtcNow && _cachedFileWrite == lastWrite)
                {
                    logger.ServingCachedBestPractices(filePath);
                    return _cachedContent;
                }

                await CacheLock.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    // Double-check after acquiring the lock
                    if (_cachedContent is not null && _cacheExpires > DateTimeOffset.UtcNow && _cachedFileWrite == lastWrite)
                    {
                        logger.ServingCachedBestPractices(filePath);
                        return _cachedContent;
                    }

                    logger.LoadingBestPractices(filePath);
                    var content = await File.ReadAllTextAsync(filePath, cancellationToken).ConfigureAwait(false);
                    _cachedContent = content;
                    _cachedFileWrite = lastWrite;
                    _cacheExpires = DateTimeOffset.UtcNow.AddMinutes(5);
                    return content;
                }
                finally
                {
                    CacheLock.Release();
                }
            }
            else
            {
                logger.BestPracticesFileNotFound(filePath);
            }
        }
        catch (OperationCanceledException)
        {
            // Propagate cancellation so the host can stop gracefully
            throw;
        }
        catch (IOException ex)
        {
            logger.FailedToLoadBestPractices(ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.FailedToLoadBestPractices(ex);
        }
        catch (NotSupportedException ex)
        {
            logger.FailedToLoadBestPractices(ex);
        }

        string[] fallback = new[]
        {
            "# C# Best Practices",
            "- Use meaningful names and clear intent.",
            "- Keep methods small and single-purpose.",
            "- Handle exceptions precisely; preserve stack traces.",
            "- Embrace async/await and cancellation.",
            "- Write tests and keep them deterministic.",
            "- Follow SOLID and prefer immutability where practical."
        };
        return string.Join(Environment.NewLine, fallback);
    }
}

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