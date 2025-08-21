# C# Best Practices

A concise, opinionated checklist for writing maintainable, reliable, and secure C# code. These are general, language-focused practices meant to complement framework- or platform-specific guidance.

## Quick checklist

- Prefer clarity over cleverness; name things for intent.
- Keep methods small; a function should do one thing well.
- Embrace async/await end-to-end; pass CancellationToken.
- Validate inputs and fail fast with helpful messages.
- Catch only what you can handle; preserve stack traces.
- Use nullable reference types and guard against nulls.
- Favor immutability (records/readonly) for data models.
- Dispose resources deterministically; prefer `await using`.
- Measure before optimizing; avoid premature micro-opts.
- Write tests for behavior and edge cases; keep them stable.

---

## Naming and style

- Use PascalCase for types, methods, and public members; camelCase for locals and parameters.
- Choose descriptive, intent-revealing names; avoid abbreviations and magic numbers.
- Prefer expression-bodied members for short, obvious implementations; otherwise keep method bodies simple.
- Keep files focused: generally one public type per file.

## Nullability and contracts

- Enable nullable reference types and fix warnings. Treat warnings as design feedback.
- Validate public method inputs with argument checks (`ArgumentNullException.ThrowIfNull(...)`).
- Document assumptions and constraints via XML docs or remarks.
- When returning reference types, prefer non-null results; use `Try` patterns to avoid exceptions for expected misses.

## Immutability and data modeling

- Prefer immutable types for value-like data (records, `init` setters, `readonly struct`).
- Avoid public mutable fields; prefer private setters or constructor injection.
- For value types that represent a concept (e.g., Money), implement `IEquatable<T>` and a consistent `GetHashCode`.

## Exceptions and error handling

- Use exceptions for exceptional conditions, not control flow. Avoid silent failures.
- Catch the most specific exceptions possible; avoid blanket `catch (Exception)` unless rethrowing.
- Preserve stack traces with `throw;` instead of `throw ex;`.
- Add context to logs (operation, identifiers) without leaking sensitive data.

## Async/await and concurrency

- Make asynchronous APIs truly async: avoid blocking (`.Result`, `.Wait()`) and thread starvation.
- Propagate `CancellationToken` to all async operations; honor cancellation promptly and cooperatively.
- Avoid `async void` except for event handlers. Prefer `Task`/`Task<T>`.
- Use `ConfigureAwait(false)` in library code that doesn’t need a context; omit in app code unless necessary.

## Collections and LINQ

- Choose the right collection for the job (e.g., `List<T>`, `Dictionary<TKey,TValue>`, `ImmutableArray<T>`).
- Prefer LINQ for readability, but be mindful of allocations and multiple enumerations; materialize once when needed.
- Avoid side effects inside LINQ queries; keep them pure.

## I/O, disposables, and resources

- Use `using`/`await using` to ensure timely disposal of `IDisposable`/`IAsyncDisposable` resources.
- Prefer streams and async I/O for scalability. Buffer thoughtfully and avoid reading entire files into memory when large.
- When exposing streams, document ownership and lifetime expectations clearly.

## Performance and memory

- Measure before optimizing; base changes on profiling data.
- Avoid unnecessary allocations in hot paths (boxing, string concatenation in loops—use `StringBuilder`).
- Prefer `Span<T>`/`Memory<T>` only when justified by profiling and with care; keep APIs safe and friendly.

## Logging, configuration, and DI

- Use structured logging (`ILogger`) with message templates and relevant context.
- Centralize configuration via options pattern; validate with `IValidateOptions<T>`.
- Prefer constructor injection; keep constructors lightweight. Avoid service location in business code.

## Testing and quality gates

- Write unit tests for behavior and edge cases; follow Arrange-Act-Assert.
- Keep tests deterministic; isolate external dependencies via interfaces and test doubles.
- Add a minimal set of integration tests for critical paths.
- Enable analyzers and treat warnings as errors where practical.

## API design

- Prefer small, cohesive interfaces (ISP). Avoid “fat” interfaces.
- Follow the principle of least surprise: consistent naming and behavior across APIs.
- Consider versioning strategy early; avoid breaking changes to public contracts.
- Prefer `TryXxx` patterns for lookups; avoid throwing for common miss conditions.

## Security and privacy

- Never log secrets or PII. Redact sensitive fields.
- Validate and sanitize untrusted input; use allow-lists where possible.
- Use platform secrets/key vaults for secret material (don’t store in config files).
- Prefer modern cryptographic primitives via `System.Security.Cryptography`; avoid custom crypto.

## Date/time and globalization

- Use `DateTimeOffset` for absolute points in time; avoid `DateTime.Now` for logic—prefer `DateTimeOffset.UtcNow`.
- Be explicit about time zones and calendar/culture assumptions.
- When parsing/formatting, specify culture (`CultureInfo.InvariantCulture`) where appropriate.

## Coding patterns and language features

- Use pattern matching and switch expressions to simplify branching.
- Apply `readonly` to fields and structs where applicable.
- Use extension methods sparingly and only when they improve readability without hiding complexity.
- Override `ToString()` for value-like types for better diagnostics; ensure it’s side-effect free.

---

## References

- .NET documentation and performance guidelines
- FxCop/CA analyzers and Roslyn code analysis rules
- Secure coding guidance from Microsoft SDL
- Azure MCP: Azure Functions codegen best practices
	- https://github.com/Azure/azure-mcp/blob/main/areas/azurebestpractices/src/AzureMcp.AzureBestPractices/Resources/azure-functions-codegen-best-practices.txt
- Azure MCP: Azure Functions deployment best practices
	- https://github.com/Azure/azure-mcp/blob/main/areas/azurebestpractices/src/AzureMcp.AzureBestPractices/Resources/azure-functions-deployment-best-practices.txt
- Azure MCP: Azure general codegen best practices
	- https://github.com/Azure/azure-mcp/blob/main/areas/azurebestpractices/src/AzureMcp.AzureBestPractices/Resources/azure-general-codegen-best-practices.txt

This document is intentionally framework-agnostic. Layer platform-specific practices (e.g., ASP.NET Core, Azure Functions) on top of these fundamentals.
