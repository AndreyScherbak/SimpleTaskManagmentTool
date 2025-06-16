using Application.Validators;
using FluentAssertions;
using Tests.Unit.Mocks;

namespace Tests.Unit.Application.Validators;

public class DueDateValidatorTests
{
    [Fact]
    public async Task Invalid_WhenPastDate()
    {
        var clock = new FakeDateTimeProvider { UtcNow = new DateTime(2024,1,1) };
        var v = new DueDateValidator(clock);
        var res = await v.ValidateAsync(new DateTime(2023,12,31));
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenTooFarInFuture()
    {
        var clock = new FakeDateTimeProvider { UtcNow = new DateTime(2024,1,1) };
        var v = new DueDateValidator(clock);
        var res = await v.ValidateAsync(new DateTime(2030,1,1));
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Valid_WhenWithinRange()
    {
        var clock = new FakeDateTimeProvider { UtcNow = new DateTime(2024,1,1) };
        var v = new DueDateValidator(clock);
        var res = await v.ValidateAsync(new DateTime(2024,2,1));
        res.Success.Should().BeTrue();
    }
}
