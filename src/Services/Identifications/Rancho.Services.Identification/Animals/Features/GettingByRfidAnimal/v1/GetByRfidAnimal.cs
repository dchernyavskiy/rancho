using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Animals.Dtos;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identification.Animals.Features.GettingByRfidAnimal.v1;

public record GetAnimalByRfid(Guid RfidTagId) : IQuery<GetAnimalByRfidResponse>;

public class GetAnimalByRfidValidator : AbstractValidator<GetAnimalByRfid>
{
    public GetAnimalByRfidValidator()
    {
        RuleFor(x => x.RfidTagId).NotEmpty();
    }
}

public class GetAnimalByRfidHandler : IQueryHandler<GetAnimalByRfid, GetAnimalByRfidResponse>
{
    private readonly IIdentificationDbContext _context;
    private readonly IMapper _mapper;


    public GetAnimalByRfidHandler(IIdentificationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAnimalByRfidResponse> Handle(GetAnimalByRfid request, CancellationToken cancellationToken)
    {
        // var entity = await _context.Animals.FirstOrDefaultAsync(x => x.RfidTagId == request.RfidTagId, cancellationToken: cancellationToken);
        var tag = await _context.RfidTags
                      .Include(x => x.Animal)
                      .FirstOrDefaultAsync(x => x.Id == request.RfidTagId, cancellationToken: cancellationToken);
        var entity = tag?.Animal;

        Guard.Against.Null(entity, nameof(Animal), $"No animal found with rfid tag id {request.RfidTagId}");

        var dto = _mapper.Map<AnimalDto>(entity);

        return new GetAnimalByRfidResponse(dto);
    }
}

public record GetAnimalByRfidResponse(AnimalDto Animal);

public class GetAnimalByRfidEndpoint : EndpointBaseAsync.WithRequest<GetAnimalByRfid>.WithoutResult
{
    private readonly IQueryProcessor _queryProcessor;

    public GetAnimalByRfidEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(AnimalConfigs.AnimalsPrefixUri, Name = "GetAnimalByRfid")]
    [ProducesResponseType(typeof(GetAnimalByRfidResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Animal By Rfid",
        Description = "Get Animal By Rfid",
        OperationId = "GetAnimalByRfid",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<GetAnimalByRfidResponse> HandleAsync(
        [FromQuery] GetAnimalByRfid request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
