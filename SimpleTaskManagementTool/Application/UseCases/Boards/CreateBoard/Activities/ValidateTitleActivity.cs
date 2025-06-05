using Application.Abstractions.Activites;
using Application.Abstractions.Models;

namespace Application.UseCases.Boards.CreateBoard.Activities
{
    public sealed class ValidateTitleActivity
     : IActivity<ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>>>
    {
        private readonly CreateBoardValidator _validator;
        public ValidateTitleActivity(CreateBoardValidator validator) => _validator = validator;

        public async Task ExecuteAsync(
            ActivityContext<CreateBoardRequest, Result<CreateBoardResponse>> ctx,
            Func<Task> next, CancellationToken ct = default)
        {
            var res = await _validator.ValidateAsync(ctx.Request, ct);
            if (!res.Success)
            {
                ctx.Result = Result<CreateBoardResponse>.Fail(res.Error!);
                return;
            }
            await next();
        }
    }
}
