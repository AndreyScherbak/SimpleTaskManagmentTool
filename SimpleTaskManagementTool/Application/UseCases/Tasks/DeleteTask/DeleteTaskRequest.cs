namespace Application.UseCases.Tasks.DeleteTask
{

    /// <summary>
    /// Command issued by the client to remove a task from a board.
    /// </summary>
    public sealed record DeleteTaskRequest(Guid BoardId, Guid TaskId);
}
