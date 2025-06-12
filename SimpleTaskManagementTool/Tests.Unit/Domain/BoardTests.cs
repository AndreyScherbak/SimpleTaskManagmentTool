using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Unit.Domain;

public class BoardTests
{
    [Fact]
    public void AddTask_AttachesTaskToBoard()
    {
        var board = new Board(Guid.NewGuid(), "Board1");
        var task = new TaskEntity("Task", DateTime.UtcNow, null);

        board.AddTask(task);

        board.Tasks.Should().ContainSingle(t => t == task);
        task.BoardId.Should().Be(board.Id);
        task.Board.Should().Be(board);
    }

    [Fact]
    public void RemoveTask_RemovesExistingTask()
    {
        var board = new Board(Guid.NewGuid(), "Board1");
        var task = new TaskEntity("Task", DateTime.UtcNow, null);
        board.AddTask(task);

        var result = board.RemoveTask(task.Id);

        result.Should().BeTrue();
        board.Tasks.Should().BeEmpty();
    }

    [Fact]
    public void RemoveTask_ReturnsFalseWhenMissing()
    {
        var board = new Board(Guid.NewGuid(), "Board1");
        var result = board.RemoveTask(Guid.NewGuid());
        result.Should().BeFalse();
    }
}
