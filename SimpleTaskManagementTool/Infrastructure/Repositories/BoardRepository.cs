using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal sealed class BoardRepository : IBoardRepository
    {
        private readonly SimpleTaskDbContext _ctx;

        public BoardRepository(SimpleTaskDbContext ctx) => _ctx = ctx;

        public async Task<Board?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _ctx.Boards
                         .Include(b => b.Tasks)          // eager-load tasks if needed
                         .FirstOrDefaultAsync(b => b.Id == id, ct);

        public async Task<IReadOnlyList<Board>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
            => await _ctx.Boards
                         .Include(b => b.Tasks)
                         .ToListAsync(ct);

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
            => await _ctx.Boards.AnyAsync(b => b.Id == id, ct);

        public async Task<Board?> GetBoardWithTaskAsync(Guid boardId, Guid taskId, CancellationToken ct = default)
            => await _ctx.Boards
                         .Include(b => b.Tasks)
                         .Where(b => b.Id == boardId)
                         .Where(b => b.Tasks.Any(t => t.Id == taskId))
                         .FirstOrDefaultAsync(ct);

        public async Task<bool> ExistsByTitleAsync(string title, CancellationToken ct = default)
            => await _ctx.Boards.AnyAsync(b => b.Title == title, ct);

        /* --------------------------------------------------------------------
         *  The next three methods only register changes in the DbContext.
         *  They do NOT hit the database until SaveChangesAsync is called.
         * ------------------------------------------------------------------ */

        public void Add(Board board)
        {
            _ctx.Boards.Add(board);
        }

        public Task UpdateAsync(Board board, CancellationToken ct = default)
        {
            _ctx.Boards.Update(board);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var board = await _ctx.Boards.FindAsync(new object[] { id }, ct);
            if (board is not null)
                _ctx.Boards.Remove(board);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _ctx.SaveChangesAsync(ct);
    }
}
