namespace Application.Abstractions.Models
{
    public record Result<T> : Result
    {
        public T? Value { get; init; }

        public static Result<T> Ok(T value) => new() { Success = true, Value = value };
        public static new Result<T> Fail(string error) => new() { Success = false, Error = error };
    }

}
