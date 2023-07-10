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
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farms.Features.GettingFarms.v1;

public record GetListOfFarms : CListQuery<GetListOfFarmsResponse>;

public record GetListOfFarmsResponse(ListResultModel<Farm> Body);

public class GetListOfFarmsHandler : IQueryHandler<GetListOfFarms, GetListOfFarmsResponse>
{
    private readonly IManagementDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetListOfFarmsHandler(IManagementDbContext context, ISecurityContextAccessor securityContextAccessor)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<GetListOfFarmsResponse> Handle(GetListOfFarms request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var farms = await _context.Farms
                        .Include(x => x.Animals)
                        .Include(x => x.Farmers)
                        .Where(x => x.OwnerId == userId)
                        .Filter(request.Filters)
                        .Sort(request.Sorts)
                        .ApplyPagingAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);

        return new GetListOfFarmsResponse(farms);
    }
}

public class GetListOfFarmsEndpoint : EndpointBaseAsync.WithRequest<GetListOfFarms>.WithActionResult
    <GetListOfFarmsResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetListOfFarmsEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FarmConfigs.FarmsPrefixUri + "/get-all", Name = "GetFarms")]
    [ProducesResponseType(typeof(GetListOfFarmsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting All Farms",
        Description = "Getting All Farms",
        OperationId = "GetFarms",
        Tags = new[]
               {
                   FarmConfigs.Tag
               })]
    public override async Task<ActionResult<GetListOfFarmsResponse>> HandleAsync(
        [FromQuery] GetListOfFarms request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
