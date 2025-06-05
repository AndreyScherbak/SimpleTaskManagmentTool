namespace Application.Abstractions.Models
{
    public record Result
    {
        public bool Success { get; init; }
        public string? Error { get; init; }

        public static Result Ok() => new() { Success = true };
        public static Result Fail(string error) => new() { Success = false, Error = error };
    }
}
