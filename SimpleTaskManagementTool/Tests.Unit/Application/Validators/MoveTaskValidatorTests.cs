using Application.UseCases.Tasks.MoveTask;
using Application.Validators;
using Domain.Entities;
using TaskStatus = Domain.Enums.TaskStatus;
using FluentAssertions;
using Tests.Unit.Mocks;

namespace Tests.Unit.Application.Validators;

public class MoveTaskValidatorTests
{
    [Fact]
    public async Task Fails_When_Board_NotFound()
    {
        var repo = new FakeBoardRepository();
        var validator = new MoveTaskValidator(repo);
        var req = new MoveTaskRequest(Guid.NewGuid(), Guid.NewGuid(), "Todo");
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Fails_When_Task_NotFound()
    {
        var repo = new FakeBoardRepository();
        var board = new Board(Guid.NewGuid(), "b");
        repo.Add(board);
        var validator = new MoveTaskValidator(repo);
        var req = new MoveTaskRequest(board.Id, Guid.NewGuid(), "Todo");
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Fails_When_InvalidTransition()
    {
        var repo = new FakeBoardRepository();
        var board = new Board(Guid.NewGuid(), "b");
        var task = new TaskEntity("t", DateTime.UtcNow, null);
        task.MoveTo(TaskStatus.Done);
        board.AddTask(task);
        repo.Add(board);
        var validator = new MoveTaskValidator(repo);
        var req = new MoveTaskRequest(board.Id, task.Id, "InProgress");
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Passes_On_Valid_Transition()
    {
        var repo = new FakeBoardRepository();
        var board = new Board(Guid.NewGuid(), "b");
        var task = new TaskEntity("t", DateTime.UtcNow, null);
        board.AddTask(task);
        repo.Add(board);
        var validator = new MoveTaskValidator(repo);
        var req = new MoveTaskRequest(board.Id, task.Id, "InProgress");
        var res = await validator.ValidateAsync(req);
        res.Success.Should().BeTrue();
    }
}
