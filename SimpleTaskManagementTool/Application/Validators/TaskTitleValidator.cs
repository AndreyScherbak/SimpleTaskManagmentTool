using Application.Abstractions.Models;
using Application.Abstractions.Validation;
using System.Text.RegularExpressions;

namespace Application.Validators
{
    /// <summary>Validates a board title for presence, length, and allowed characters.</summary>
    /// <summary>Validates a task title (non-empty, length, chars).</summary>
    public sealed class TaskTitleValidator : IValidator<string>
    {
        private const int MinLength = 3;
        private const int MaxLength = 150;
        private static readonly Regex Allowed = new(@"^[\w\s\-]+$", RegexOptions.Compiled);

        public Task<Result> ValidateAsync(string title, CancellationToken _ = default)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Task.FromResult(Result.Fail("Task title must not be empty."));

            var trimmed = title.Trim();

            if (trimmed.Length < MinLength || trimmed.Length > MaxLength)
                return Task.FromResult(Result.Fail($"Task title length must be between {MinLength} and {MaxLength} characters."));

            if (!Allowed.IsMatch(trimmed))
                return Task.FromResult(Result.Fail("Task title contains invalid characters."));

            return Task.FromResult(Result.Ok());
        }
    }
}
