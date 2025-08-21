using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

using System.IO;

namespace BlankSlate.Functions;

public class McpTools(ILogger<McpTools> logger)
{
    [Function(nameof(GetBestPractices))]
    public string GetBestPractices(
        [McpToolTrigger("get_csharp_best_practices", "Retrieves C# best practices")]
            ToolInvocationContext toolContext)
    {
        logger.LogInformation("Serving C# best practices via MCP tool {ToolName}", "get_csharp_best_practices");
        try
        {
            // Resolve path relative to the function app's working directory
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "csharp-best-practices.md");
            if (File.Exists(filePath))
            {
                logger.LogDebug("Loading best practices from {FilePath}", filePath);
                return File.ReadAllText(filePath);
            }
            else
            {
                logger.LogWarning("Best practices file not found at {FilePath}; serving fallback", filePath);
            }
        }
        catch
        {
            // Ignore and fall back to inline defaults below
        }

        var fallback = new[]
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