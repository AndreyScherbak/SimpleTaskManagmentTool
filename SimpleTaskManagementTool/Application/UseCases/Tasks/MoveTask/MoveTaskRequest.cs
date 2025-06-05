namespace Application.UseCases.Tasks.MoveTask
{
    public sealed record MoveTaskRequest(Guid BoardId, Guid TaskId, string TargetStatus);
}
