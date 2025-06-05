namespace Application.UseCases.Tasks.CreateTask
{
    public sealed record CreateTaskRequest(Guid BoardId, string Title, DateTime? DueDate);
}
