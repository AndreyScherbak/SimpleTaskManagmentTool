namespace Application.UseCases.Tasks.Common
{
    /// <summary>
    /// Read-model DTO for returning task information from Task use-cases.
    /// Enriches the core <see cref="Abstractions.DTOs.TaskDto"/> with <paramref name="BoardId"/>.
    /// </summary>
    public sealed record TaskDto(
        Guid Id,
        Guid BoardId,
        string Title,
        DateTime CreatedAt,
        DateTime? DueDate,
        string Status);
}
