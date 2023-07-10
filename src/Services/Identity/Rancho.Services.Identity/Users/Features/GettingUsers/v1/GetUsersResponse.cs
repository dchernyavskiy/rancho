using BuildingBlocks.Core.CQRS.Queries;
using Rancho.Services.Identity.Users.Dtos;
using Rancho.Services.Identity.Users.Dtos.v1;

namespace Rancho.Services.Identity.Users.Features.GettingUsers.v1;

public record GetUsersResponse(ListResultModel<IdentityUserDto> IdentityUsers);
