namespace Application.Abstractions.DTOs
{
    public record CreateTaskRequest(string Title, DateTime? DueDate);
}
