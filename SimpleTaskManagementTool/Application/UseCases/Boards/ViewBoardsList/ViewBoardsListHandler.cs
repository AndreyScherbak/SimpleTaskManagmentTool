using Application.Abstractions.Models;
using Application.Abstractions.Services;
using Application.Abstractions.UseCases;
using Application.UseCases.Boards.CreateBoard;
using Domain.Interfaces;

namespace Application.UseCases.Boards.ViewBoardsList
{
    public sealed class ViewBoardsListHandler
     : IUseCase<object?, Result<ViewBoardsListResponse>>   // no request data needed
    {
        private readonly IBoardRepository _boardRepo;
        private readonly IUserContextService _userCtx;

        public ViewBoardsListHandler(IBoardRepository boardRepo, IUserContextService userCtx)
        {
            _boardRepo = boardRepo;
            _userCtx = userCtx;
        }

        public async Task<Result<ViewBoardsListResponse>> ExecuteAsync(object? _, CancellationToken ct = default)
        {
            var userId = _userCtx.GetCurrentUserId();
            var boards = await _boardRepo.GetAllByUserIdAsync(userId, ct);

            var dtos = boards.Select(b => new BoardDto(b.Id, b.Title, b.Tasks.Count))
                             .ToList();

            return Result<ViewBoardsListResponse>.Ok(new(dtos));
        }
    }
}
