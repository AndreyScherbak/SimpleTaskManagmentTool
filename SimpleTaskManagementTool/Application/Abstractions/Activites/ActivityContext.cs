namespace Application.Abstractions.Activites
{
    public sealed class ActivityContext<TRequest, TResult>
    {
        public TRequest Request { get; }
        public TResult? Result { get; set; }
        public IDictionary<string, object?> Items { get; } = new Dictionary<string, object?>();

        public ActivityContext(TRequest request, TResult? result = default) =>
            (Request, Result) = (request, result);
    }
}
