namespace Application.Abstractions.Services
{
    public interface IUserContextService
    {
        Guid GetCurrentUserId();
    }
}
