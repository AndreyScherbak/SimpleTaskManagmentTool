using Application.Abstractions.Models;
using Application.Abstractions.Validation;
using Application.Validators;
using Domain.Interfaces;

namespace Application.UseCases.Boards.CreateBoard
{
    public sealed class CreateBoardValidator : IValidator<CreateBoardRequest>
    {
        private readonly BoardTitleValidator _titleValidator;
        private readonly IBoardRepository _boardRepo;

        public CreateBoardValidator(BoardTitleValidator titleValidator,
                                    IBoardRepository boardRepo)
        {
            _titleValidator = titleValidator;
            _boardRepo = boardRepo;
        }

        public async Task<Result> ValidateAsync(CreateBoardRequest req, CancellationToken ct = default)
        {
            var titleRes = await _titleValidator.ValidateAsync(req.Title, ct);
            if (!titleRes.Success) return Result.Fail(titleRes.Error!);

            var exists = await _boardRepo.ExistsByTitleAsync(req.Title, ct);
            if (exists) return Result.Fail($"Board with title '{req.Title}' already exists.");

            return Result.Ok();
        }
    }
}
