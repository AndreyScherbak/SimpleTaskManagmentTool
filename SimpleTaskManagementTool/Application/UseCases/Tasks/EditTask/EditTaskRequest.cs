namespace Application.UseCases.Tasks.EditTask
{
    public sealed record EditTaskRequest(
    Guid BoardId,
    Guid TaskId,
    string Title,
    DateTime? DueDate);
}
