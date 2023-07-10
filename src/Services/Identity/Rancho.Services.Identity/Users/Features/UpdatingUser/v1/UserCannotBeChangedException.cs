using System.Net;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Types.Extensions;
using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Users.Features.UpdatingUser.v1;

internal class UserCannotBeChangedException : AppException
{
    public UserState State { get; }
    public Guid UserId { get; }

    public UserCannotBeChangedException(UserState state, Guid userId)
        : base(
            $"User state cannot be changed to: '{state.ToName()}' for user with ID: '{userId}'.",
            HttpStatusCode.InternalServerError
        )
    {
        State = state;
        UserId = userId;
    }
}
