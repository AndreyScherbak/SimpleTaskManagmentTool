using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Abstractions.UseCases;

namespace Application.UseCases.Tasks.MoveTask
{
    public sealed class MoveTaskHandler
    : IUseCase<MoveTaskRequest, Result<MoveTaskResponse>>
    {
        private readonly IActivityFactory<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>> _factory;

        public MoveTaskHandler(IActivityFactory<ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>> factory)
            => _factory = factory;

        public async Task<Result<MoveTaskResponse>> ExecuteAsync(
            MoveTaskRequest request,
            CancellationToken cancellationToken = default)
        {
            var context = new ActivityContext<MoveTaskRequest, Result<MoveTaskResponse>>(request, null!);

            // resolve pipeline (Load → Validate → Update → Persist)
            var pipeline = _factory.CreatePipeline().ToArray();

            // link the delegates
            Func<Task> terminal = () => Task.CompletedTask;
            for (int i = pipeline.Length - 1; i >= 0; i--)
            {
                var activity = pipeline[i];
                var next = terminal;
                terminal = () => activity.ExecuteAsync(context, next, cancellationToken);
            }

            await terminal();

            return context.Result!;
        }
    }
}
