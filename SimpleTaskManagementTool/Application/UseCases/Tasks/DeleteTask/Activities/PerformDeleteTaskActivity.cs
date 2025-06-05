using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.UseCases.Tasks.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.DeleteTask.Activities
{
    public sealed class PerformDeleteTaskActivity :
    IActivity<ActivityContext<DeleteTaskRequest, Result<TaskDto>>>
    {
        private readonly IBoardRepository _boardRepo;

        public PerformDeleteTaskActivity(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<DeleteTaskRequest, Result<TaskDto>> context,
            Func<Task> next,
            CancellationToken cancellationToken = default)
        {
            if (!context.Items.TryGetValue("Board", out var boardObj) ||
                !context.Items.TryGetValue("Task", out var taskObj))
            {
                // Safety net – should never happen if pipeline is wired correctly
                context = new(context.Request,
                              Result<TaskDto>.Fail("Internal pipeline error (missing Board/Task)."));
                return;
            }

            var board = (Board)boardObj!;
            var task = (TaskEntity)taskObj!;

            board.RemoveTask(task.Id);

            await _boardRepo.SaveChangesAsync(cancellationToken);

            var dto = new TaskDto(task.Id,
                                  board.Id,
                                  task.Title,
                                  task.CreatedAt,
                                  task.DueDate,
                                  task.Status.ToString());

            context = new(context.Request, Result<TaskDto>.Ok(dto));

            await next(); // tail-call for consistency
        }
    }
}
