using Application.Abstractions.Models;
using Application.Abstractions.Services;
using Application.Abstractions.Validation;

namespace Application.Validators
{
    /// <summary>Ensures the due-date is in the future and within a reasonable horizon.</summary>
    public sealed class DueDateValidator : IValidator<DateTime>
    {
        private readonly IDateTimeProvider _clock;

        public DueDateValidator(IDateTimeProvider clock) => _clock = clock;

        public Task<Result> ValidateAsync(DateTime dueDate, CancellationToken _ = default)
        {
            var now = _clock.UtcNow;

            if (dueDate.Date < now.Date)
                return Task.FromResult(Result.Fail("Due-date cannot be in the past."));

            // e.g. allow at most 5 years into future (arbitrary business rule)
            if (dueDate.Date > now.Date.AddYears(5))
                return Task.FromResult(Result.Fail("Due-date is unreasonably far in the future."));

            return Task.FromResult(Result.Ok());
        }
    }
}
