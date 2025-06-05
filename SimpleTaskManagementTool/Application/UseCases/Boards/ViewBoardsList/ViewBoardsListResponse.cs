using Application.UseCases.Boards.CreateBoard;

namespace Application.UseCases.Boards.ViewBoardsList
{
    public sealed record ViewBoardsListResponse(IReadOnlyCollection<BoardDto> Boards);
}
