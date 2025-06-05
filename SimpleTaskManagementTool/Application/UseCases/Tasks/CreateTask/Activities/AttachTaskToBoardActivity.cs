using Application.Abstractions.Activites;
using Application.UseCases.Tasks.Common;
using Application.Abstractions.Models;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.CreateTask.Activities
{
    /// <summary>
    /// Creates the <see cref="Task"/> domain object and associates it with the target board in memory.
    /// Persists will occur in a later activity (<see cref="SaveTaskActivity"/>).
    /// </summary>
    public sealed class AttachTaskToBoardActivity : IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>
    {
        private readonly IBoardRepository _boardRepo;
        private readonly IDateTimeProvider _clock;

        public AttachTaskToBoardActivity(IBoardRepository boardRepo, IDateTimeProvider clock)
        {
            _boardRepo = boardRepo;
            _clock = clock;
        }

        public async Task ExecuteAsync(ActivityContext<CreateTaskRequest, Result<TaskDto>> context,
                                       Func<Task> next,
                                       CancellationToken cancellationToken = default)
        {
            var board = await _boardRepo.GetByIdAsync(context.Request.BoardId, cancellationToken);
            if (board is null)
            {
                context.Result = Result<TaskDto>.Fail($"Board '{context.Request.BoardId}' not found.");
                return;
            }

            var task = new TaskEntity(context.Request.Title, _clock.UtcNow, context.Request.DueDate);
            board.AddTask(task);

            // Pass task forward via context by wrapping in Result if pipeline completes prematurely
            await next();
        }
    }
}
