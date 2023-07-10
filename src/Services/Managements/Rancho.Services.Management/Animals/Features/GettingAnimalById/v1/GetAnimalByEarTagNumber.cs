using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Features.GettingListOfAnimals.v1;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Animals.Features.GettingAnimalById.v1;

public record GetAnimalById(Guid Id) : IQuery<GetAnimalByIdResponse>;

public class GetAnimalByRfidHandler : IQueryHandler<GetAnimalById, GetAnimalByIdResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;

    public GetAnimalByRfidHandler(IManagementDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAnimalByIdResponse> Handle(
        GetAnimalById request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Animals
                         .FirstOrDefaultAsync(
                             x => x.Id == request.Id,
                             cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Animal was not found.");

        var entityDto = _mapper.Map<AnimalDto>(entity);

        return new GetAnimalByIdResponse(entityDto);
    }
}

public record GetAnimalByIdResponse(AnimalDto Animal);
