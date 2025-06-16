using System.Security.Claims;
using Application.Abstractions.Services;

namespace WebApi.Services;

/// <summary>
/// Retrieves the current user id from the HTTP context.
/// </summary>
public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var id = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
    }
}
