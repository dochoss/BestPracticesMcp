using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace BlankSlate.Functions;

/// <summary>
/// Provides tools for retrieving best practices for Python development.
/// Utilizes a process-wide cache to minimize disk reads and improve performance.
/// </summary>
/// <remarks>
/// This class is intended for internal use and is designed to be used within a function app context.
/// It reads best practices from a markdown file and caches the content for a short duration.
/// </remarks>
public class PythonTools(ILogger<PythonTools> logger)
{
    // Simple process-wide cache to avoid disk reads on every invocation
    private static readonly SemaphoreSlim CacheLock = new(1, 1);
    private static string? _cachedContent;
    private static DateTimeOffset _cachedFileWrite;
    private static DateTimeOffset _cacheExpires;

    [Function(nameof(GetPythonBestPractices))]
    public async Task<string> GetPythonBestPractices(
        [McpToolTrigger("get_python_best_practices", "Retrieves best practices for Python development")]
            ToolInvocationContext toolContext,
        CancellationToken cancellationToken)
    {
        logger.ServingPythonBestPractices("get_python_best_practices");
        try
        {
            // Resolve path relative to the function app's working directory
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "python-best-practices.md");
            if (File.Exists(filePath))
            {
                var lastWrite = File.GetLastWriteTimeUtc(filePath);

                // Return cached if still valid and file unchanged
                if (_cachedContent is not null && _cacheExpires > DateTimeOffset.UtcNow && _cachedFileWrite == lastWrite)
                {
                    logger.ServingCachedPythonBestPractices(filePath);
                    return _cachedContent;
                }

                await CacheLock.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    // Double-check after acquiring the lock
                    if (_cachedContent is not null && _cacheExpires > DateTimeOffset.UtcNow && _cachedFileWrite == lastWrite)
                    {
                        logger.ServingCachedPythonBestPractices(filePath);
                        return _cachedContent;
                    }

                    logger.LoadingPythonBestPractices(filePath);
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
                logger.PythonBestPracticesFileNotFound(filePath);
            }
        }
        catch (OperationCanceledException)
        {
            // Propagate cancellation so the host can stop gracefully
            throw;
        }
        catch (IOException ex)
        {
            logger.FailedToLoadPythonBestPractices(ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.FailedToLoadPythonBestPractices(ex);
        }
        catch (NotSupportedException ex)
        {
            logger.FailedToLoadPythonBestPractices(ex);
        }

        string[] fallback = new[]
        {
            "# Python Best Practices",
            "- Follow PEP 8 style guidelines; use automated formatters.",
            "- Write self-documenting code with clear, descriptive names.",
            "- Use type hints for function signatures and complex data structures.",
            "- Prefer list/dict comprehensions and generators for readability.",
            "- Handle exceptions explicitly; use specific exception types.",
            "- Write docstrings for modules, classes, and public functions.",
            "- Use virtual environments and pin dependencies with requirements files.",
            "- Follow the principle of least surprise; be explicit rather than implicit.",
            "- Write unit tests with meaningful assertions and good coverage.",
            "- Use logging instead of print statements for debugging and monitoring."
        };
        return string.Join(Environment.NewLine, fallback);
    }
}

// LoggerMessage source-generated helpers for better perf and analyzer compliance
internal static partial class PythonToolsLogs
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Serving Python best practices via MCP tool {ToolName}")]
    public static partial void ServingPythonBestPractices(this ILogger logger, string toolName);

    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "Loading Python best practices from {FilePath}")]
    public static partial void LoadingPythonBestPractices(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 3, Level = LogLevel.Debug, Message = "Serving cached Python best practices content from {FilePath}")]
    public static partial void ServingCachedPythonBestPractices(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "Python best practices file not found at {FilePath}; serving fallback")]
    public static partial void PythonBestPracticesFileNotFound(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "Failed to load Python best practices content; serving fallback")]
    public static partial void FailedToLoadPythonBestPractices(this ILogger logger, Exception exception);
}