using System.Diagnostics;

namespace Application.Common
{
    public sealed class ExecutionContext
    {
        public Guid CorrelationId { get; }
        public Guid UserId { get; }
        public DateTime UtcTimestamp { get; }

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public ExecutionContext(Guid userId, Guid? correlationId = null, DateTime? timestampUtc = null)
        {
            UserId = userId;
            CorrelationId = correlationId ?? Guid.NewGuid();
            UtcTimestamp = timestampUtc ?? DateTime.UtcNow;
        }

        /// <summary>
        /// Elapsed wall-clock time since the <see cref="ExecutionContext"/> was created.
        /// </summary>
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        /// <summary>
        /// Creates a shallow copy with a new user id—useful for system / service calls.
        /// </summary>
        public ExecutionContext WithUser(Guid newUserId) =>
            new(newUserId, CorrelationId, UtcTimestamp);

        public override string ToString() =>
            $"[CorrelationId: {CorrelationId}] UserId={UserId}, Created={UtcTimestamp:O}";
    }
}
