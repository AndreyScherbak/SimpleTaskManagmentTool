namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Task?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Task>> GetAllByBoardIdAsync(Guid boardId);
        Task AddAsync(Task task);
        Task UpdateAsync(Task task);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

    }
}
