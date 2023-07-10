using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Mapping;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Animals.Features.GettingListOfAnimals.v1;

public record GetListOfAnimals : CListQuery<GetListOfAnimalsResponse>;

public class GetListOfAnimalsHandler : IQueryHandler<GetListOfAnimals, GetListOfAnimalsResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetListOfAnimalsHandler(
        IManagementDbContext context,
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor
    )
    {
        _context = context;
        _mapper = mapper;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<GetListOfAnimalsResponse> Handle(
        GetListOfAnimals request,
        CancellationToken cancellationToken
    )
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);
        var userRole = _securityContextAccessor.Role;

        IQueryable<Animal> animals = default!;
        if (userRole == ManagementConstants.Role.User || userRole == ManagementConstants.Role.Farmer)
        {
            animals = _context.Farmers
                .Where(x => x.Id == userId)
                .Select(x => x.Farm)
                .SelectMany(x => x.Animals);
            var aa = await _context.Farmers
                .Where(x => x.Id == userId)
                .Include(x => x.Farm)
                .ThenInclude(x => x.Animals)
                .ToListAsync(cancellationToken: cancellationToken);
            var s = await animals.ToListAsync(cancellationToken: cancellationToken);
        }
        else
        {
            animals = _context.Animals
                .Include(x => x.Farm)
                .Where(x => x.Farm.OwnerId == userId);
        }

        var result = await animals.Filter(request.Filters)
                         .Sort(request.Sorts)
                         .ProjectTo<AnimalDto>(_mapper.ConfigurationProvider)
                         .ApplyPagingAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);


        return new GetListOfAnimalsResponse(result);
    }
}

public record GetListOfAnimalsResponse(ListResultModel<AnimalDto> Body);
