using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Rancho.Services.Identity.Users.Features.RegisteringUser.v1;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identity.Users.Features.UpdatingUser.v1;

public static class UpdateUserEndpoint
{
    internal static RouteHandlerBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/user", UpdateUser)
            .AllowAnonymous()
            .Produces<RegisterUserResponse>(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("UpdateUser")
            .WithDisplayName("Update User.")
            .WithMetadata(new SwaggerOperationAttribute("Updating User.", "Updating User"));
    }

    private static async Task<IResult> UpdateUser(
        [FromBody] UpdateUser request,
        ICommandProcessor commandProcessor,
        CancellationToken cancellationToken
    )
    {
        await commandProcessor.SendAsync(request, cancellationToken);

        return Results.NoContent();
    }
}
