using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.UseCases.Tasks.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.MoveTask.Activities
{
    public sealed class PersistStatusChangeActivity
     : IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>
    {
        private readonly IBoardRepository _boardRepo;

        public PersistStatusChangeActivity(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>> context,
            Func<Task> next,
            CancellationToken ct = default)
        {
            // Persist board/task status change
            await _boardRepo.SaveChangesAsync(ct);

            // Reload the board WITH the task to map to DTOs
            var savedBoard = await _boardRepo.GetBoardWithTaskAsync(
                context.Request.BoardId,
                context.Request.TaskId,
                ct);

            if (savedBoard is null)
            {
                context.Result = Result<MoveTaskResponse>.Fail("Board or task not found after save.");
                return;
            }

            var task = savedBoard.Tasks.First(t => t.Id == context.Request.TaskId);

            var taskDto = new TaskDto(
                task.Id,
                savedBoard.Id,
                task.Title,
                task.CreatedAt,
                task.DueDate,
                task.Status.ToString());

            context.Result = Result<MoveTaskResponse>.Ok(new MoveTaskResponse(taskDto));

            await next();
        }
    }
}
