using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.EditTask.Activities
{
    public sealed class LoadTaskActivity
     : IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>
    {
        private readonly IBoardRepository _boardRepo;

        public LoadTaskActivity(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<EditTaskRequest, Result<EditTaskResponse>> context,
            Func<Task> next,
            CancellationToken ct = default)
        {
            var board = await _boardRepo.GetByIdAsync(context.Request.BoardId, ct);
            if (board is null)
            {
                context.Result = Result<EditTaskResponse>.Fail($"Board '{context.Request.BoardId}' not found.");
                return;
            }

            var task = board.Tasks.SingleOrDefault(t => t.Id == context.Request.TaskId);
            if (task is null)
            {
                context.Result = Result<EditTaskResponse>.Fail($"Task '{context.Request.TaskId}' not found.");
                return;
            }

            context.Items["Board"] = board;
            context.Items["Task"] = task;

            await next();
        }
    }
}
