using Application.UseCases.Boards.CreateBoard;
using Application.Validators;
using FluentAssertions;
using Tests.Unit.Mocks;

namespace Tests.Unit.Application.Validators;

public class CreateBoardValidatorTests
{
    [Fact]
    public async Task Fails_When_Title_Already_Exists()
    {
        var repo = new FakeBoardRepository();
        repo.Add(new global::Domain.Entities.Board(Guid.NewGuid(), "Existing"));
        var validator = new CreateBoardValidator(new BoardTitleValidator(), repo);
        var req = new CreateBoardRequest("Existing");
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Passes_For_Unique_Title()
    {
        var repo = new FakeBoardRepository();
        var validator = new CreateBoardValidator(new BoardTitleValidator(), repo);
        var req = new CreateBoardRequest("New Board");
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeTrue();
    }
}
