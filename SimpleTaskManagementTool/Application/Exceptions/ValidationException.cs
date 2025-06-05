using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    /// <summary>
    /// Thrown when <see cref="Result"/> indicates failure originating from explicit business-rule / data-rule validation.
    /// </summary>
    [Serializable]
    public sealed class ValidationException : Exception
    {
        /// <summary>Individual validation error messages.</summary>
        public IReadOnlyCollection<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : this("One or more validation errors occurred.", errors) { }

        public ValidationException(string message, IEnumerable<string> errors)
            : base(message) =>
            Errors = new ReadOnlyCollection<string>(errors.ToArray());

        public ValidationException(string message)
            : base(message) =>
            Errors = Array.Empty<string>();

        public ValidationException(string message, Exception innerException)
            : base(message, innerException) =>
            Errors = Array.Empty<string>();

        private ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Errors = (string[])(info.GetValue(nameof(Errors), typeof(string[])) ?? Array.Empty<string>());
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Errors), Errors.ToArray());
        }
    }
}
