using Application.Abstractions.Activites;
using Application.UseCases.Tasks.Common;
using Application.Abstractions.Models;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.CreateTask.Activities
{
    public sealed class SaveTaskActivity
    : IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>
    {
        private readonly IBoardRepository _boardRepo;

        public SaveTaskActivity(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<CreateTaskRequest, Result<TaskDto>> context,
            Func<Task> next,
            CancellationToken cancellationToken = default)
        {
            // persist board + task
            await _boardRepo.SaveChangesAsync(cancellationToken);

            // reload to map DTO (or map directly if you still have the entity)
            var savedBoard = await _boardRepo.GetByIdAsync(context.Request.BoardId, cancellationToken);
            var savedTask = savedBoard?.Tasks.LastOrDefault(t => t.Title == context.Request.Title);

            if (savedTask is null)
            {
                context.Result = Result<TaskDto>.Fail("Task could not be persisted.");
                return;
            }

            var dto = new TaskDto(savedTask.Id,
                      savedBoard!.Id,
                      savedTask.Title,
                      savedTask.CreatedAt,
                      savedTask.DueDate,
                      savedTask.Status.ToString());

            context.Result = Result<TaskDto>.Ok(dto);
            await next();
        }
    }

}
