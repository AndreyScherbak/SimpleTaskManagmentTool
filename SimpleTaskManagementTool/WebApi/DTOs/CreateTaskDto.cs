namespace WebApi.DTOs;

/// <summary>
/// Payload for creating or updating a task.
/// </summary>
public sealed record CreateTaskDto(string Title, DateTime? DueDate);
