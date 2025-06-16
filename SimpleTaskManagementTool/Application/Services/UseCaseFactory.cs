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
        public Task<TResult> ExecuteAsync<TRequest, TResult, TUseCase>(
            TRequest request,
            CancellationToken ct = default)
            where TUseCase : IUseCase<TRequest, TResult>
            where TResult : Result
        {
            // Resolve the concrete use case and delegate execution to it. The
            // use case itself is responsible for running its activity pipeline.
            var useCase = _provider.GetRequiredService<TUseCase>();
            return useCase.ExecuteAsync(request, ct);
        }
    }
}
