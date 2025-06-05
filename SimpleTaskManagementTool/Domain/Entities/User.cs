using Domain.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string DisplayName { get; private set; }

        public User(string email, string displayName)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(displayName))
                throw new DomainException("Email and Display Name are required.");

            Id = Guid.NewGuid();
            Email = email.Trim();
            DisplayName = displayName.Trim();
        }
    }
}
