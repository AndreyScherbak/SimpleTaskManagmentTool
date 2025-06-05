using System.Runtime.Serialization;

namespace Application.Exceptions
{
    /// <summary>
    /// Represents an unexpected, non-validation failure during a use-case handler’s execution
    /// (e.g., a downstream service crashed, a repository threw, etc.).
    /// </summary>
    [Serializable]
    public sealed class UseCaseExecutionException : Exception
    {
        public UseCaseExecutionException() { }

        public UseCaseExecutionException(string message)
            : base(message) { }

        public UseCaseExecutionException(string message, Exception innerException)
            : base(message, innerException) { }

        // For (de)serialization support — required when the exception might cross AppDomain / process boundaries.
        private UseCaseExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
