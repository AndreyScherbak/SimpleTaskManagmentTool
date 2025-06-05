using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public readonly struct TaskTitle
    {
        public string Value { get; }

        public TaskTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Task title cannot be empty.");
            Value = value.Trim();
        }

        public override string ToString() => Value;
    }
}
