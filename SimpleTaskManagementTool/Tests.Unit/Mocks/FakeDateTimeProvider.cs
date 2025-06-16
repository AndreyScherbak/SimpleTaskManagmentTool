using Application.Abstractions.Services;

namespace Tests.Unit.Mocks;

internal sealed class FakeDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get; set; }
}
