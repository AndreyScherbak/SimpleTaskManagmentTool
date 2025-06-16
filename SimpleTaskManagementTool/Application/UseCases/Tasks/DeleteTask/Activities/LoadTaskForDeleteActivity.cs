using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.UseCases.Tasks.Common;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.DeleteTask.Activities
{
    public sealed class LoadTaskForDeleteActivity :
    IActivity<ActivityContext<DeleteTaskRequest, Result<TaskDto>>>
    {
        private readonly IBoardRepository _boardRepo;

        public LoadTaskForDeleteActivity(IBoardRepository boardRepo) =>
            _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<DeleteTaskRequest, Result<TaskDto>> context,
            Func<Task> next,
            CancellationToken cancellationToken = default)
        {
            var board = await _boardRepo.GetByIdAsync(context.Request.BoardId, cancellationToken);

            if (board is null)
            {
                context.Result = Result<TaskDto>.Fail($"Board '{context.Request.BoardId}' not found.");
                return;
            }

            var task = board.Tasks.SingleOrDefault(t => t.Id == context.Request.TaskId);

            if (task is null)
            {
                context.Result = Result<TaskDto>.Fail($"Task '{context.Request.TaskId}' not found on board.");
                return;
            }

            // Pass the domain instances to the PerformDeleteActivity via the execution context
            context.Items["Board"] = board;
            context.Items["Task"] = task;

            await next();
        }
    }
}
