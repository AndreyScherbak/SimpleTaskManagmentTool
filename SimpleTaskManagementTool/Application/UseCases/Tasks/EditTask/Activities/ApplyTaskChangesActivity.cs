using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.UseCases.Tasks.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.EditTask.Activities
{
    public sealed class ApplyTaskChangesActivity
    : IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>
    {
        private readonly IBoardRepository _boardRepo;

        public ApplyTaskChangesActivity(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<EditTaskRequest, Result<EditTaskResponse>> context,
            Func<Task> next,
            CancellationToken ct = default)
        {
            var board = (Board)context.Items["Board"]!;
            var task = (TaskEntity)context.Items["Task"]!;

            // Apply changes
            task.SetTitle(context.Request.Title);
            task.SetDueDate(context.Request.DueDate);

            await _boardRepo.SaveChangesAsync(ct);

            var dto = new TaskDto(task.Id, board.Id, task.Title, task.CreatedAt, task.DueDate, task.Status.ToString());
            context.Result = Result<EditTaskResponse>.Ok(new EditTaskResponse(dto));

            await next();   // usually tail-call, but keeps pipeline pattern
        }
    }
}
