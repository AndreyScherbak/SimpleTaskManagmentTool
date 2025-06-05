using Application.Abstractions.Activites;
using Application.Abstractions.Models;

namespace Application.UseCases.Tasks.MoveTask.Activities
{
    public sealed class ValidateStatusTransitionActivity
    : IActivity<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>>
    {
        private readonly MoveTaskValidator _validator;
        public ValidateStatusTransitionActivity(MoveTaskValidator validator) => _validator = validator;

        public async Task ExecuteAsync(
            ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>> context,
            Func<Task> next,
            CancellationToken ct = default)
        {
            var validation = await _validator.ValidateAsync(context.Request, ct);
            if (!validation.Success)
            {
                context.Result = Result<MoveTaskResponse>.Fail(validation.Error!);
                return;
            }

            await next();
        }
    }
}
