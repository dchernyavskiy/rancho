using AutoMapper;
using Rancho.Services.Identity.Users.Dtos;
using Rancho.Services.Identity.Shared.Models;
using Rancho.Services.Identity.Users.Dtos.v1;

namespace Rancho.Services.Identity.Users;

public class UsersMapping : Profile
{
    public UsersMapping()
    {
        CreateMap<ApplicationUser, IdentityUserDto>()
            .ForMember(x => x.RefreshTokens, opt => opt.MapFrom(x => x.RefreshTokens.Select(r => r.Token)))
            .ForMember(
                x => x.Roles,
                opt => opt.MapFrom(x => x.UserRoles.Where(m => m.Role != null).Select(q => q.Role!.Name))
            );
    }
}
