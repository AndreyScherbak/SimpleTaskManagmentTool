using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.CreateBoard.Activities
{
    public sealed class SaveBoardActivity
     : IActivity<ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>>
    {
        private readonly IBoardRepository _boardRepo;

        public SaveBoardActivity(IBoardRepository boardRepo)
            => _boardRepo = boardRepo;

        public async Task ExecuteAsync(
            ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>> ctx,
            Func<Task> next,
            CancellationToken ct = default)
        {
            // ►  Use the **current** Board constructor (Id, Title)
            var board = new Board(Guid.NewGuid(), ctx.Request.Title.Trim());

            _boardRepo.Add(board);
            await _boardRepo.SaveChangesAsync(ct);

            var dto = new BoardDto(board.Id, board.Title, /*taskCount*/ 0);
            ctx.Result = Result<CreateBoardResponse>.Ok(new(dto));

            await next();   // nothing after this, but keeps the pipeline contract
        }
    }
}
