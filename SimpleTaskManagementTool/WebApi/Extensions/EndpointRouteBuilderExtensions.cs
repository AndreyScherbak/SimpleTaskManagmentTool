using Microsoft.AspNetCore.Builder;

namespace WebApi.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder app)
    {
        app.MapControllers();
        return app;
    }
}
