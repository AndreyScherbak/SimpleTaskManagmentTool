using Domain.Entities;
using TaskStatus = Domain.Enums.TaskStatus;
using FluentAssertions;

namespace Tests.Unit.Domain;

public class TaskItemTests
{
    [Fact]
    public void Constructor_SetsDefaults()
    {
        var now = DateTime.UtcNow;
        var task = new TaskEntity("title", now, null);

        task.Title.Should().Be("title");
        task.CreatedAt.Should().Be(now);
        task.Status.Should().Be(TaskStatus.Todo);
    }

    [Fact]
    public void SetTitle_TrimsAndUpdates()
    {
        var task = new TaskEntity("title", DateTime.UtcNow, null);
        task.SetTitle("  new title  ");
        task.Title.Should().Be("new title");
    }

    [Fact]
    public void MoveTo_ChangesStatus()
    {
        var task = new TaskEntity("title", DateTime.UtcNow, null);
        task.MoveTo(TaskStatus.Done);
        task.Status.Should().Be(TaskStatus.Done);
    }

    [Fact]
    public void AttachToBoard_SetsNavigation()
    {
        var board = new Board(Guid.NewGuid(), "b");
        var task = new TaskEntity("t", DateTime.UtcNow, null);
        board.AddTask(task);
        task.Board.Should().Be(board);
        task.BoardId.Should().Be(board.Id);
    }
}
