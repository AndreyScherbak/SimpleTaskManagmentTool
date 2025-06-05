using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.MoveTask.Activities
{
    public sealed class LoadTaskActivity
     : IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>
    {
        private readonly IBoardRepository _boardRepo;

        public LoadTaskActivity(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>> context,
            Func<Task> next,
            CancellationToken cancellationToken = default)
        {
            var board = await _boardRepo.GetByIdAsync(context.Request.BoardId, cancellationToken);

            if (board is null)
            {
                context.Result = Result<MoveTaskResponse>.Fail(
                    $"Board with id '{context.Request.BoardId}' was not found.");
                return; 
            }

            var task = board.Tasks.SingleOrDefault(t => t.Id == context.Request.TaskId);

            if (task is null)
            {
                context.Result = Result<MoveTaskResponse>.Fail(
                    $"Task with id '{context.Request.TaskId}' was not found on board '{board.Id}'.");
                return;
            }

            context.Items["Board"] = board;
            context.Items["Task"] = task;

            await next();
        }
    }
}
