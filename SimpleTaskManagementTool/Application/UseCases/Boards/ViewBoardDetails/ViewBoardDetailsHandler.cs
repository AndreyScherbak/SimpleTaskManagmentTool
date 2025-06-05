using Application.Abstractions.Models;
using Application.Abstractions.UseCases;
using Application.UseCases.Boards.CreateBoard;
using Application.UseCases.Tasks.Common;
using Domain.Interfaces;

namespace Application.UseCases.Boards.ViewBoardDetails
{
    public sealed class ViewBoardDetailsHandler
     : IUseCase<Guid, Result<ViewBoardDetailsResponse>>
    {
        private readonly IBoardRepository _boardRepo;

        public ViewBoardDetailsHandler(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task<Result<ViewBoardDetailsResponse>> ExecuteAsync(Guid boardId, CancellationToken ct = default)
        {
            var board = await _boardRepo.GetByIdAsync(boardId, ct);
            if (board is null)
                return Result<ViewBoardDetailsResponse>.Fail($"Board '{boardId}' not found.");

            var boardDto = new BoardDto(board.Id, board.Title, board.Tasks.Count);
            var taskDtos = board.Tasks
                                .Select(t => new TaskDto(t.Id, board.Id, t.Title, t.CreatedAt, t.DueDate, t.Status.ToString()))
                                .ToList();

            return Result<ViewBoardDetailsResponse>.Ok(new(boardDto, taskDtos));
        }
    };
}
