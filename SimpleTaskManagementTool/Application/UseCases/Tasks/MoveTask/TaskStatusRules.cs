using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.UseCases.Tasks.MoveTask
{
    internal static class TaskStatusRules
    {
        private static readonly IReadOnlyDictionary<TaskStatus, TaskStatus[]> Map =
            new Dictionary<TaskStatus, TaskStatus[]>
            {
                [TaskStatus.Todo] = new[] { TaskStatus.InProgress },
                [TaskStatus.InProgress] = new[] { TaskStatus.Done, TaskStatus.Todo },
                [TaskStatus.Done] = Array.Empty<TaskStatus>()
            };

        public static bool IsValidTransition(TaskStatus from, TaskStatus to) =>
            Map.TryGetValue(from, out var next) && next.Contains(to);
    }
}
