using Application.Abstractions.Activites;
using Application.Abstractions.Models;
using Application.Abstractions.UseCases;

namespace Application.UseCases.Boards.CreateBoard
{
    public sealed class CreateBoardHandler
    : IUseCase<CreateBoardRequest, Result<CreateBoardResponse>>
    {
        private readonly IEnumerable<IActivity<
            ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>>> _pipeline;

        public CreateBoardHandler(IEnumerable<IActivity<
            ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>>> pipeline)
            => _pipeline = pipeline;

        public async Task<Result<CreateBoardResponse>> ExecuteAsync(
            CreateBoardRequest req, CancellationToken ct = default)
        {
            var ctx = new ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>(req, null);

            using var iter = _pipeline.GetEnumerator();
            Task Next() => iter.MoveNext()
                ? iter.Current.ExecuteAsync(ctx, Next, ct)
                : Task.CompletedTask;

            await Next();
            return ctx.Result ?? Result<CreateBoardResponse>.Fail("Pipeline produced no result.");
        }
    }
}
