using Application.Abstractions.Activites;
using Application.Abstractions.DTOs;
using Application.Abstractions.Models;

namespace Application.UseCases.Tasks.CreateTask.Activities
{
    public sealed class ValidateTaskDetailsActivity : IActivity<ActivityContext<CreateTaskRequest, Result<TaskDto>>>
    {
        private readonly CreateTaskValidator _validator;

        public ValidateTaskDetailsActivity(CreateTaskValidator validator) => _validator = validator;

        public async Task ExecuteAsync(ActivityContext<CreateTaskRequest, Result<TaskDto>> context,
                                       Func<Task> next,
                                       CancellationToken cancellationToken = default)
        {
            var validation = await _validator.ValidateAsync(context.Request, cancellationToken);
            if (!validation.Success)
            {
                context.Result = Result<TaskDto>.Fail(validation.Error!);
                return; // short‑circuit
            }

            await next();
        }
    }
}
