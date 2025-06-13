using Application.Validators;
using FluentAssertions;

namespace Tests.Unit.Application.Validators;

public class TaskTitleValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task Invalid_WhenEmpty(string? title)
    {
        var validator = new TaskTitleValidator();
        var result = await validator.ValidateAsync(title!);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenTooShort()
    {
        var validator = new TaskTitleValidator();
        var result = await validator.ValidateAsync("ab");
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenTooLong()
    {
        var validator = new TaskTitleValidator();
        var longTitle = new string('a', 151);
        var result = await validator.ValidateAsync(longTitle);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenContainsIllegalChars()
    {
        var validator = new TaskTitleValidator();
        var result = await validator.ValidateAsync("Bad!");
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Valid_Title()
    {
        var validator = new TaskTitleValidator();
        var result = await validator.ValidateAsync("Valid Title");
        result.Success.Should().BeTrue();
    }
}
