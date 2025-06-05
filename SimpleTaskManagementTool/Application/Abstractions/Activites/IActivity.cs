namespace Application.Abstractions.Activites
{
    public interface IActivity<TContext>
    {
        Task ExecuteAsync(TContext context, Func<Task> next, CancellationToken cancellationToken = default);
    }
}
