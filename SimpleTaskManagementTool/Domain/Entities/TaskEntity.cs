using TaskStatus = Domain.Enums.TaskStatus;
namespace Domain.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? DueDate { get; private set; }
        public TaskStatus Status { get; private set; }
        public Guid BoardId { get; private set; }
        public Board? Board { get; private set; }

        public TaskEntity(string title, DateTime createdAt, DateTime? dueDate)
        {
            Id = new Guid();
            Title = title;
            CreatedAt = createdAt;
            DueDate = dueDate;
            Status = TaskStatus.Todo;
        }

        public void SetTitle(string title) => Title = title.Trim();
        public void SetDueDate(DateTime? due) => DueDate = due;
        public void MoveTo(TaskStatus newStatus) => Status = newStatus;

        internal void AttachToBoard(Board board)
        {
            Board = board;
            BoardId = board.Id;
        }
    }
}
