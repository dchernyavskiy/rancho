using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identification.Animals.Features.GettingAnimals.v1;

public record GetListOfAnimals() : CListQuery<GetListOfAnimalsResponse>;

public record GetListOfAnimalsResponse(ListResultModel<Animal> Body);

public class GetListOfAnimalsHandler : IQueryHandler<GetListOfAnimals, GetListOfAnimalsResponse>
{
    private readonly IIdentificationDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly ILogger<GetListOfAnimalsHandler> _logger;

    public GetListOfAnimalsHandler(IIdentificationDbContext context, ISecurityContextAccessor securityContextAccessor, ILogger<GetListOfAnimalsHandler> logger)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
        _logger = logger;
    }

    public async Task<GetListOfAnimalsResponse> Handle(GetListOfAnimals request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);


        var animals = await _context.Farms
                          .Include(x => x.Animals)
                          .Where(x => x.OwnerId == userId)
                          .SelectMany(x => x.Animals)
                          .ApplyPagingAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);

        return new GetListOfAnimalsResponse(animals);
    }
}


public class GetListOfAnimalsEndpoint : EndpointBaseAsync.WithRequest<GetListOfAnimals>.WithoutResult
{
    private readonly IQueryProcessor _queryProcessor;

    public GetListOfAnimalsEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(AnimalConfigs.AnimalsPrefixUri + "/get-all", Name = "GetListOfAnimals")]
    [ProducesResponseType(typeof(GetListOfAnimalsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get List Of Animals",
        Description = "Get List Of Animals",
        OperationId = "GetListOfAnimals",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<GetListOfAnimalsResponse> HandleAsync(
        [FromQuery] GetListOfAnimals request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
