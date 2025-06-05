namespace Application.Abstractions.Activites
{
    public interface IActivityFactory<TContext>
    {
        IEnumerable<IActivity<TContext>> CreatePipeline();
    }
}
