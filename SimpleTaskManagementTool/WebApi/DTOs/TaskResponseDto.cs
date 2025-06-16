namespace WebApi.DTOs;

/// <summary>
/// DTO returned from task endpoints.
/// </summary>
public sealed record TaskResponseDto(
    Guid Id,
    Guid BoardId,
    string Title,
    DateTime CreatedAt,
    DateTime? DueDate,
    string Status);
