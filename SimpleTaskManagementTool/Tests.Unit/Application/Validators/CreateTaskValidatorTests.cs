using Application.UseCases.Tasks.CreateTask;
using Application.Validators;
using FluentAssertions;
using Tests.Unit.Mocks;

namespace Tests.Unit.Application.Validators;

public class CreateTaskValidatorTests
{
    [Fact]
    public async Task Fails_When_Board_NotFound()
    {
        var repo = new FakeBoardRepository();
        var validator = new CreateTaskValidator(new TaskTitleValidator(), new DueDateValidator(new FakeDateTimeProvider{UtcNow=DateTime.UtcNow}), repo);
        var req = new CreateTaskRequest(Guid.NewGuid(), "task", null);
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Passes_With_ValidData()
    {
        var repo = new FakeBoardRepository();
        var board = new global::Domain.Entities.Board(Guid.NewGuid(), "Board");
        repo.Add(board);
        var clock = new FakeDateTimeProvider{UtcNow=DateTime.UtcNow};
        var validator = new CreateTaskValidator(new TaskTitleValidator(), new DueDateValidator(clock), repo);
        var req = new CreateTaskRequest(board.Id, "New Task", clock.UtcNow.AddDays(1));
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeTrue();
    }
}
