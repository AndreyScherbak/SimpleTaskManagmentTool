namespace Application.Abstractions.DTOs
{
    public record UpdateTaskRequest(Guid Id, string Title, DateTime? DueDate);
}
