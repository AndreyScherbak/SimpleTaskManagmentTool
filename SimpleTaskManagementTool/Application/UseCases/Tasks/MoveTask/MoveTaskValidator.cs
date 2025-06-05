using Application.Abstractions.Models;
using Application.Abstractions.Validation;
using Domain.Interfaces;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.UseCases.Tasks.MoveTask
{
    public sealed class MoveTaskValidator : IValidator<MoveTaskRequest>
    {
        private readonly IBoardRepository _boardRepo;

        public MoveTaskValidator(IBoardRepository boardRepo) => _boardRepo = boardRepo;

        public async Task<Result> ValidateAsync(MoveTaskRequest request, CancellationToken ct = default)
        {
            var board = await _boardRepo.GetByIdAsync(request.BoardId, ct);
            if (board is null)
                return Result.Fail($"Board '{request.BoardId}' not found.");

            var task = board.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
            if (task is null)
                return Result.Fail($"Task '{request.TaskId}' not found on board.");

            if (!Enum.TryParse<TaskStatus>(request.TargetStatus, true, out var target))
                return Result.Fail($"Unknown target status '{request.TargetStatus}'.");

            if (!TaskStatusRules.IsValidTransition(task.Status, target))
                return Result.Fail($"Cannot move task from {task.Status} to {target}.");

            return Result.Ok();
        }
    }
}
