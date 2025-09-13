using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace BlankSlate.Functions;

/// <summary>
/// Provides tools for retrieving best practices for Vue 3 development.
/// Utilizes a process-wide cache to minimize disk reads and improve performance.
/// </summary>
/// <remarks>
/// This class is intended for internal use and is designed to be used within a function app context.
/// It reads best practices from a markdown file and caches the content for a short duration.
/// </remarks>
public class Vue3Tools(ILogger<Vue3Tools> logger)
{
    // Simple process-wide cache to avoid disk reads on every invocation
    private static readonly SemaphoreSlim CacheLock = new(1, 1);
    private static string? _cachedContent;
    private static DateTimeOffset _cachedFileWrite;
    private static DateTimeOffset _cacheExpires;

    [Function(nameof(GetVue3BestPractices))]
    public async Task<string> GetVue3BestPractices(
        [McpToolTrigger("get_vue3_best_practices", "Retrieves best practices for Vue 3 development")]
            ToolInvocationContext toolContext,
        CancellationToken cancellationToken)
    {
        logger.ServingVue3BestPractices("get_vue3_best_practices");
        try
        {
            // Resolve path relative to the function app's working directory
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "vue3-best-practices.md");
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
                        logger.ServingCachedVue3BestPractices(filePath);
                        return _cachedContent;
                    }

                    logger.LoadingVue3BestPractices(filePath);
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
                logger.Vue3BestPracticesFileNotFound(filePath);
            }
        }
        catch (OperationCanceledException)
        {
            // Propagate cancellation so the host can stop gracefully
            throw;
        }
        catch (IOException ex)
        {
            logger.FailedToLoadVue3BestPractices(ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.FailedToLoadVue3BestPractices(ex);
        }
        catch (NotSupportedException ex)
        {
            logger.FailedToLoadVue3BestPractices(ex);
        }

        string[] fallback = new[]
        {
            "# Vue 3 Best Practices",
            "- Use multi-word component names to avoid conflicts.",
            "- Define detailed prop types with validation.",
            "- Always use :key with v-for for predictable rendering.",
            "- Avoid v-if with v-for on the same element.",
            "- Use component-scoped styling (scoped CSS or CSS modules).",
            "- Prefer Composition API for better TypeScript support.",
            "- Keep template expressions simple; use computed properties for complex logic.",
            "- Follow consistent component naming and file organization."
        };
        return string.Join(Environment.NewLine, fallback);
    }
}

// LoggerMessage source-generated helpers for better perf and analyzer compliance
internal static partial class Vue3ToolsLogs
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Serving Vue 3 best practices via MCP tool {ToolName}")]
    public static partial void ServingVue3BestPractices(this ILogger logger, string toolName);

    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "Loading Vue 3 best practices from {FilePath}")]
    public static partial void LoadingVue3BestPractices(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 3, Level = LogLevel.Debug, Message = "Serving cached Vue 3 best practices content from {FilePath}")]
    public static partial void ServingCachedVue3BestPractices(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "Vue 3 best practices file not found at {FilePath}; serving fallback")]
    public static partial void Vue3BestPracticesFileNotFound(this ILogger logger, string filePath);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "Failed to load Vue 3 best practices content; serving fallback")]
    public static partial void FailedToLoadVue3BestPractices(this ILogger logger, Exception exception);
}