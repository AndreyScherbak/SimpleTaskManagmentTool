namespace Application.Abstractions.UseCases
{
    public interface IUseCase<in TRequest, TResult>
    {
        Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
