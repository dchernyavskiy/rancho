using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Features.GettingListOfAnimals.v1;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Animals.Features.GettingAnimalByEarTagNumber.v1;

public record GetAnimalByEarTagNumber(string EarTagNumber) : IQuery<GetAnimalByEarTagNumberResponse>;

public class GetAnimalByRfidHandler : IQueryHandler<GetAnimalByEarTagNumber, GetAnimalByEarTagNumberResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;

    public GetAnimalByRfidHandler(IManagementDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAnimalByEarTagNumberResponse> Handle(
        GetAnimalByEarTagNumber request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Animals
                         .FirstOrDefaultAsync(
                             x => x.EarTagNumber == request.EarTagNumber,
                             cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Animal was not found.");

        var entityDto = _mapper.Map<AnimalDto>(entity);

        return new GetAnimalByEarTagNumberResponse(entityDto);
    }
}

public record GetAnimalByEarTagNumberResponse(AnimalDto Animal);
