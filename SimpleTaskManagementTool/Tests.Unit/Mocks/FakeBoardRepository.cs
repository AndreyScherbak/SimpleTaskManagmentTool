using Domain.Entities;
using Domain.Interfaces;

namespace Tests.Unit.Mocks;

internal sealed class FakeBoardRepository : IBoardRepository
{
    private readonly List<Board> _boards = new();

    public IReadOnlyList<Board> Boards => _boards;

    public void Add(Board board) => _boards.Add(board);

    public Task<Board?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult<Board?>(_boards.FirstOrDefault(b => b.Id == id));

    public Task<Board?> GetBoardWithTaskAsync(Guid boardId, Guid taskId, CancellationToken ct = default)
        => Task.FromResult<Board?>(
            _boards.FirstOrDefault(b => b.Id == boardId && b.Tasks.Any(t => t.Id == taskId)));

    public Task<IReadOnlyList<Board>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        => Task.FromResult((IReadOnlyList<Board>)_boards.ToList());

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;

    public Task<bool> ExistsByTitleAsync(string title, CancellationToken ct = default)
        => Task.FromResult(_boards.Any(b => b.Title == title));
}
