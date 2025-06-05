using Domain.Exceptions;

namespace Domain.Entities
{

    public class Board
    {
        private readonly List<TaskEntity> _tasks = new();

        public Guid Id { get; }
        public string Title { get; private set; }

        public IReadOnlyCollection<TaskEntity> Tasks => _tasks.AsReadOnly();

        public Board(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public void AddTask(TaskEntity task)
        {
            task.AttachToBoard(this);
            _tasks.Add(task);
        }

        public bool RemoveTask(Guid taskId)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (existing is null) return false;
            _tasks.Remove(existing);
            return true;
        }
    }
}
