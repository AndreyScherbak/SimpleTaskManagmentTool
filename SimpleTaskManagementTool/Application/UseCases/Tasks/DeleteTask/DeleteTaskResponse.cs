using Application.UseCases.Tasks.Common;

namespace Application.UseCases.Tasks.DeleteTask
{
    /// <summary>
    /// Returned when a task has been deleted.  
    /// Includes the details of the deleted task for confirmation / undo UI.
    /// </summary>
    public sealed record DeleteTaskResponse(TaskDto DeletedTask);
}
