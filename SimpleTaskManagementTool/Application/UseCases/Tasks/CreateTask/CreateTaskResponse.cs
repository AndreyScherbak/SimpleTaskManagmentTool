using Application.UseCases.Tasks.Common;

namespace Application.UseCases.Tasks.CreateTask
{
    /// <summary>
    /// Returned when a task has been successfully created.
    /// </summary>
    /// <param name="Task">Projection of the persisted domain entity.</param>
    public sealed record CreateTaskResponse(TaskDto Task);
}
