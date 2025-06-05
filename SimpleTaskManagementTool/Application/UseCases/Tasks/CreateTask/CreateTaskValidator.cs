using Application.Abstractions.Models;
using Application.Abstractions.Validation;
using Application.Validators;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.CreateTask
{
    /// <summary>
    /// Validates the <see cref="CreateTaskRequest"/> in a holistic manner by delegating to individual value validators
    /// and checking business‑level rules (e.g., board existence, user access).
    /// </summary>
    public sealed class CreateTaskValidator : IValidator<CreateTaskRequest>
    {
        private readonly TaskTitleValidator _titleValidator;
        private readonly DueDateValidator _dueDateValidator;
        private readonly IBoardRepository _boardRepo;

        public CreateTaskValidator(TaskTitleValidator titleValidator,
                                   DueDateValidator dueDateValidator,
                                   IBoardRepository boardRepo)
        {
            _titleValidator = titleValidator;
            _dueDateValidator = dueDateValidator;
            _boardRepo = boardRepo;
        }

        public async Task<Result> ValidateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            // Title & DueDate syntactic checks
            var titleResult = await _titleValidator.ValidateAsync(request.Title, cancellationToken);
            if (!titleResult.Success) return Result.Fail(titleResult.Error!);

            if (request.DueDate.HasValue)
            {
                var dueDateResult = await _dueDateValidator.ValidateAsync(request.DueDate.Value, cancellationToken);
                if (!dueDateResult.Success) return Result.Fail(dueDateResult.Error!);
            }

            // Board existence
            var boardExists = await _boardRepo.ExistsAsync(request.BoardId, cancellationToken);
            if (!boardExists)
                return Result.Fail($"Board with id '{request.BoardId}' was not found.");

            return Result.Ok();
        }
    }
}
