using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBoardRepository
    {
        Task<Board?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Board?> GetBoardWithTaskAsync(Guid boardId, Guid taskId, CancellationToken ct = default);
        Task<IReadOnlyList<Board>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid boardId, CancellationToken ct = default)
            => GetByIdAsync(boardId, ct).ContinueWith(t => t.Result is not null, ct);
        Task<bool> ExistsByTitleAsync(string title, CancellationToken ct = default);
        void Add(Board board);
    }
}
