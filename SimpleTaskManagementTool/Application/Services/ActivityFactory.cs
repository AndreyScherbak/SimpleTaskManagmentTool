using Application.Abstractions.Activites;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    /// <summary>
    /// Runtime implementation of <see cref="IActivityFactory{TContext}"/> that relies on the
    /// DI container to resolve all <see cref="IActivity{TContext}"/> registered for the
    /// requested pipeline <typeparamref name="TContext"/>.
    /// </summary>
    public sealed class ActivityFactory<TContext> : IActivityFactory<TContext>
    {
        private readonly IServiceProvider _provider;

        public ActivityFactory(IServiceProvider provider) => _provider = provider;

        public IEnumerable<IActivity<TContext>> CreatePipeline()
        {
            // All activities for this context are registered as IEnumerable in DI.
            // Activities can optionally define an IOrderedActivity marker to control ordering.
            var activities = _provider.GetServices<IActivity<TContext>>().ToArray();

            // If the caller hasn’t specified an explicit order via IOrderedActivity
            // the original registration order is preserved.
            return activities
                    .OrderBy(a =>
                        a is IOrderedActivity ordered
                        ? ordered.Order
                        : int.MaxValue);
        }
    }

    /// <summary>
    /// Optional marker interface that an activity can implement to indicate
    /// pipeline order (<see cref="IOrderedActivity.Order"/>; lower = earlier).
    /// </summary>
    public interface IOrderedActivity
    {
        int Order { get; }
    }
}
