using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Abstractions.UseCases;
using Application.UseCases.Tasks.Common;

namespace Application.UseCases.Tasks.DeleteTask
{
    public sealed class DeleteTaskHandler :
    IUseCase<DeleteTaskRequest, Result<DeleteTaskResponse>>
    {
        private readonly IList<IActivity<ActivityContext<DeleteTaskRequest, Result<TaskDto>>>> _pipeline;

        public DeleteTaskHandler(
            Activities.LoadTaskForDeleteActivity loadTask,
            Activities.PerformDeleteTaskActivity performDelete)
        {
            // Order matters:
            _pipeline = new List<IActivity<ActivityContext<DeleteTaskRequest, Result<TaskDto>>>>
        {
            loadTask,
            performDelete
        };
        }

        public async Task<Result<DeleteTaskResponse>> ExecuteAsync(
            DeleteTaskRequest request,
            CancellationToken cancellationToken = default)
        {
            // Build an activity context object
            var ctx = new ActivityContext<DeleteTaskRequest, Result<TaskDto>>(request, null!);

            // Simple manual middleware loop
            var enumerator = _pipeline.GetEnumerator();
            async Task InvokeNext()
            {
                if (enumerator.MoveNext())
                {
                    await enumerator.Current.ExecuteAsync(ctx, InvokeNext, cancellationToken);
                }
            }

            await InvokeNext();

            return ctx.Result switch
            {
                { Success: true, Value: var dto } => Result<DeleteTaskResponse>.Ok(new DeleteTaskResponse(dto!)),
                { Success: false, Error: var err } => Result<DeleteTaskResponse>.Fail(err!),
                _ => Result<DeleteTaskResponse>.Fail("Unknown execution state.")
            };
        }
    }
}
