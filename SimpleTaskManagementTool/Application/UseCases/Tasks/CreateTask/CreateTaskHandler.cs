using Application.Abstractions.Activites;
using Application.UseCases.Tasks.Common;
using Application.Abstractions.Models;
using Application.Abstractions.UseCases;

namespace Application.UseCases.Tasks.CreateTask
{
    public class CreateTaskHandler : IUseCase<CreateTaskRequest, Result<TaskDto>>
    {
        private readonly IActivityFactory<ActivityContext<CreateTaskRequest, Result<TaskDto>>> _factory;

        public CreateTaskHandler(IActivityFactory<ActivityContext<CreateTaskRequest, Result<TaskDto>>> factory)
        {
            _factory = factory;
        }

        public async Task<Result<TaskDto>> ExecuteAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var context = new ActivityContext<CreateTaskRequest, Result<TaskDto>>(request, null);
            var pipeline = _factory.CreatePipeline();

            using var enumerator = pipeline.GetEnumerator();

            async Task Next()
            {
                if (enumerator.MoveNext())
                {
                    await enumerator.Current.ExecuteAsync(context, Next, cancellationToken);
                }
            }

            await Next();

            return context.Result ?? Result<TaskDto>.Fail("Unhandled pipeline execution.");
        }
    }
}
