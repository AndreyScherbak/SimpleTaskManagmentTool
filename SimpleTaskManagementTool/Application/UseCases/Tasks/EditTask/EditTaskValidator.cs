using Application.Abstractions.Models;
using Application.Abstractions.Validation;
using Application.Validators;
using Domain.Interfaces;

namespace Application.UseCases.Tasks.EditTask
{
    public sealed class EditTaskValidator : IValidator<EditTaskRequest>
    {
        private readonly TaskTitleValidator _titleValidator;
        private readonly DueDateValidator _dueDateValidator;
        private readonly IBoardRepository _boardRepo;

        public EditTaskValidator(TaskTitleValidator titleValidator,
                                  DueDateValidator dueDateValidator,
                                  IBoardRepository boardRepo)
        {
            _titleValidator = titleValidator;
            _dueDateValidator = dueDateValidator;
            _boardRepo = boardRepo;
        }

        public async Task<Result> ValidateAsync(EditTaskRequest req, CancellationToken ct = default)
        {
            // Syntactic rules
            var titleRes = await _titleValidator.ValidateAsync(req.Title, ct);
            if (!titleRes.Success) return Result.Fail(titleRes.Error!);

            if (req.DueDate.HasValue)
            {
                var ddRes = await _dueDateValidator.ValidateAsync(req.DueDate.Value, ct);
                if (!ddRes.Success) return Result.Fail(ddRes.Error!);
            }

            // Board must exist
            if (!await _boardRepo.ExistsAsync(req.BoardId, ct))
                return Result.Fail($"Board '{req.BoardId}' not found.");

            return Result.Ok();
        }
    }
}
