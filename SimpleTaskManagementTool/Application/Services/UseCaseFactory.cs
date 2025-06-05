using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Abstractions.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    /// <summary>
    /// Helper that builds a use-case execution delegate with its activity pipeline.
    /// The factory is *not* mandatory — it simply centralises the composition logic.
    /// </summary>
    /// <summary>
    /// Builds and executes a use-case pipeline (use-case + activities).
    /// </summary>
    public sealed class UseCaseFactory
    {
        private readonly IServiceProvider _provider;

        public UseCaseFactory(IServiceProvider provider) => _provider = provider;

        /// <inheritdoc cref="IUseCase{TRequest,TResult}.ExecuteAsync"/>
        public async Task<TResult> ExecuteAsync<TRequest, TResult, TUseCase>(
            TRequest request,
            CancellationToken ct = default)
            where TUseCase : IUseCase<TRequest, TResult>
            where TResult : Result
        {
            // Resolve concrete use-case
            var useCase = _provider.GetRequiredService<TUseCase>();

            // Create context and (optionally) retrieve activities
            var ctx = new ActivityContext<TRequest, TResult>(request);

            var factory = _provider.GetService<
                IActivityFactory<ActivityContext<TRequest, TResult>>>();

            var activities = factory?.CreatePipeline().ToArray()
                          ?? Array.Empty<IActivity<ActivityContext<TRequest, TResult>>>();

            // Tail handler: run the use-case and store its result
            Func<Task> handler = async () =>
            {
                ctx.Result = await useCase.ExecuteAsync(request, ct);
            };

            // Wrap the tail with each activity (reverse order)
            foreach (var activity in activities.Reverse())
            {
                var next = handler;
                handler = () => activity.ExecuteAsync(ctx, next, ct);
            }

            // Run the composed chain
            await handler();

            return ctx.Result!;
        }
    }
}
