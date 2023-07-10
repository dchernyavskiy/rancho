using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Authentication;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farms.Features.GettingFarm.v1;

public record GetFarm(Guid Id) : IQuery<GetFarmResponse>;

public record GetFarmResponse(Farm Farm);

public class GetFarmHandler : IQueryHandler<GetFarm, GetFarmResponse>
{
    private readonly IManagementDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetFarmHandler(IManagementDbContext context, ISecurityContextAccessor securityContextAccessor)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<GetFarmResponse> Handle(GetFarm request, CancellationToken cancellationToken)
    {
        var userId = _securityContextAccessor.UserId;
        var role = _securityContextAccessor.Role;
        if (role == ManagementConstants.Role.User || role == ManagementConstants.Role.Farmer)
        {
            return new GetFarmResponse(
                (await _context.Farmers
                     .Include(x => x.Farm)
                     .ThenInclude(x => x.Animals)
                     .FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId)))?.Farm
                );
        }

        var farm = await _context.Farms
                       .Include(x => x.Farmers)
                       .Include(x => x.Animals)
                       .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        return new GetFarmResponse(farm);
    }
}

public class GetFarmEndpoint : EndpointBaseAsync.WithRequest<GetFarm>.WithActionResult
    <GetFarmResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFarmEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FarmConfigs.FarmsPrefixUri, Name = "GetFarm")]
    [ProducesResponseType(typeof(GetFarmResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting Farm",
        Description = "Getting Farm",
        OperationId = "GetFarm",
        Tags = new[]
               {
                   FarmConfigs.Tag
               })]
    public override async Task<ActionResult<GetFarmResponse>> HandleAsync(
        [FromQuery] GetFarm request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
