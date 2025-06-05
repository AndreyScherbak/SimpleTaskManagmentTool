
using Application.UseCases.Boards.CreateBoard;
using Application.UseCases.Tasks.Common;

namespace Application.UseCases.Boards.ViewBoardDetails
{
    public sealed record ViewBoardDetailsResponse(BoardDto Board, IReadOnlyCollection<TaskDto> Tasks);
}
