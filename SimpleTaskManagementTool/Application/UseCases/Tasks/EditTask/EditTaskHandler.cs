using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Abstractions.UseCases;

namespace Application.UseCases.Tasks.EditTask
{
    public sealed class EditTaskHandler
    : IUseCase<EditTaskRequest, Result<EditTaskResponse>>
    {
        private readonly IEnumerable<IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>> _pipeline;

        public EditTaskHandler(IEnumerable<IActivity<ActivityContext<EditTaskRequest, Result<EditTaskResponse>>>> pipeline)
            => _pipeline = pipeline;

        public async Task<Result<EditTaskResponse>> ExecuteAsync(EditTaskRequest request, CancellationToken ct = default)
        {
            var ctx = new ActivityContext<EditTaskRequest, Result<EditTaskResponse>>(request, null);

            // Run pipeline sequentially
            using var enumerator = _pipeline.GetEnumerator();
            Task Next() => enumerator.MoveNext()
                ? enumerator.Current.ExecuteAsync(ctx, Next, ct)
                : Task.CompletedTask;

            await Next();

            return ctx.Result ?? Result<EditTaskResponse>.Fail("Edit-task pipeline produced no result.");
        }
    }
}
