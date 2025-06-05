using Application.Abstractions.Activites;
using Application.Abstractions.Models;

namespace Application.UseCases.Tasks.EditTask.Activities
{
    public sealed class ValidateEditTaskActivity
     : IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>
    {
        private readonly EditTaskValidator _validator;

        public ValidateEditTaskActivity(EditTaskValidator validator) => _validator = validator;

        public async Task ExecuteAsync(
            ActivityContext<EditTaskRequest, Result<EditTaskResponse>> context,
            Func<Task> next,
            CancellationToken ct = default)
        {
            var res = await _validator.ValidateAsync(context.Request, ct);
            if (!res.Success)
            {
                context.Result = Result<EditTaskResponse>.Fail(res.Error!);
                return;
            }

            await next();
        }
    }
}
