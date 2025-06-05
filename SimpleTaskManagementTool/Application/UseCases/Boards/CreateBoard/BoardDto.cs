namespace Application.UseCases.Boards.CreateBoard
{
    /// <summary>Lightweight DTO returned by board-related queries.</summary>
    public sealed record BoardDto(
     Guid Id,
     string Title,
     int TasksCount);
}
