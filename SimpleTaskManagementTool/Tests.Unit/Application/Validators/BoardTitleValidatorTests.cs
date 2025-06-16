using Application.Validators;
using FluentAssertions;

namespace Tests.Unit.Application.Validators;

public class BoardTitleValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task Invalid_WhenEmpty(string? title)
    {
        var v = new BoardTitleValidator();
        var res = await v.ValidateAsync(title!);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenTooShort()
    {
        var v = new BoardTitleValidator();
        var res = await v.ValidateAsync("ab");
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenTooLong()
    {
        var v = new BoardTitleValidator();
        var title = new string('a', 101);
        var res = await v.ValidateAsync(title);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Invalid_WhenContainsIllegalChars()
    {
        var v = new BoardTitleValidator();
        var res = await v.ValidateAsync("Bad!");
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Valid_Title()
    {
        var v = new BoardTitleValidator();
        var res = await v.ValidateAsync("Project Board");
        res.Success.Should().BeTrue();
    }
}
