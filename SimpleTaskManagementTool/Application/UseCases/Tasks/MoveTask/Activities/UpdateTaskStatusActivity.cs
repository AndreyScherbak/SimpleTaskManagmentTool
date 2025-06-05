using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Domain.Entities;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.UseCases.Tasks.MoveTask.Activities
{
    public sealed class UpdateTaskStatusActivity
     : IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>
    {
        public Task ExecuteAsync(
            ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>> context,
            Func<Task> next,
            CancellationToken cancellationToken = default)
        {
            var task = (TaskEntity)context.Items["Task"]!;
            var target = Enum.Parse<TaskStatus>(context.Request.TargetStatus, ignoreCase: true);

            task.MoveTo(target);
            return next();
        }
    }
}
