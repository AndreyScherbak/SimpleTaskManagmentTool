using Application.Abstractions.Models;

namespace Application.Abstractions.Validation
{
    public interface IValidator<in TRequest>
    {
        Task<Result> ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
