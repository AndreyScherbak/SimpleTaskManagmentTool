namespace Application.Abstractions.DTOs
{
    public record TaskDto(Guid Id, string Title, DateTime CreatedAt, DateTime? DueDate, string Status);
}
