using Application.Abstractions.Models;
using Application.Abstractions.Validation;

namespace Application.Validators
{
    /// <summary>
    /// Checks whether the current user is allowed to access / mutate a given board.
    /// </summary>
    public sealed class UserAccessValidator : IValidator<Guid>
    {
        public Task<Result> ValidateAsync(Guid request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}
