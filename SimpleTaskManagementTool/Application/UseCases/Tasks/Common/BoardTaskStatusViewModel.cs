using System.Collections.ObjectModel;

namespace Application.UseCases.Tasks.Common
{
    /// <summary>
    /// Grouping helper returned by queries such as “ViewBoardDetails” or “ViewTasksByBoard”.
    /// It bundles all tasks in a board that share the same status (Todo / In-Progress / Done / etc.).
    /// </summary>
    /// <param name="Status">The textual status (value object or enum ToString()).</param>
    /// <param name="Tasks">Read-only collection of tasks in that status bucket.</param>
    public sealed record BoardTaskStatusViewModel(
        string Status,
        ReadOnlyCollection<TaskDto> Tasks);
}
