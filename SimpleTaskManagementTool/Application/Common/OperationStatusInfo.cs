namespace Application.Common
{
    /// <summary>
    /// Returned by background jobs, service bus handlers, scheduled tasks, etc.,
    /// where a DTO payload is not needed but structured status information is.
    /// For payload-rich responses, we normally use <see cref="Abstractions.Models.Result{T}"/>.
    /// </summary>
    public sealed record OperationStatusInfo
    {
        /// <summary>The correlation id from <see cref="ExecutionContext"/>.</summary>
        public Guid CorrelationId { get; init; }

        /// <summary>Whether the operation succeeded.</summary>
        public bool Success { get; init; }

        /// <summary>A human-readable status description or error message.</summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>Additional, structured data (e.g., key–value diagnostics).</summary>
        public IReadOnlyDictionary<string, object?>? Metadata { get; init; }

        public static OperationStatusInfo Ok(Guid correlationId, string? message = null,
                                             IReadOnlyDictionary<string, object?>? metadata = null) =>
            new() { CorrelationId = correlationId, Success = true, Message = message ?? "OK", Metadata = metadata };

        public static OperationStatusInfo Fail(Guid correlationId, string error,
                                               IReadOnlyDictionary<string, object?>? metadata = null) =>
            new() { CorrelationId = correlationId, Success = false, Message = error, Metadata = metadata };
    }
}
