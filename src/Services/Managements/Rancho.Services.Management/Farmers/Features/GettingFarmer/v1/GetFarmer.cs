using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farmers.Features.GettingFarmer.v1;

public record GetFarmer(Guid Id) : IQuery<GetFarmerResponse>;

public record GetFarmerResponse(Farmer Farmer);

public class GetFarmerHandler : IQueryHandler<GetFarmer, GetFarmerResponse>
{
    private readonly IManagementDbContext _context;

    public GetFarmerHandler(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task<GetFarmerResponse> Handle(GetFarmer request, CancellationToken cancellationToken)
    {
        var farmer = await _context.Farmers
                         .Include(x => x.Works)
                         .FirstOrDefaultAsync(x => x.Id == request.Id);

        return new GetFarmerResponse(farmer);
    }
}

public class GetFarmerEndpoint : EndpointBaseAsync.WithRequest<GetFarmer>.WithActionResult
    <GetFarmerResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFarmerEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FarmerConfigs.FarmersPrefixUri, Name = "GetFarmer")]
    [ProducesResponseType(typeof(GetFarmerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting Farmer",
        Description = "Getting Farmer",
        OperationId = "GetFarmer",
        Tags = new[]
               {
                   FarmerConfigs.Tag
               })]
    public override async Task<ActionResult<GetFarmerResponse>> HandleAsync(
        [FromQuery] GetFarmer request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
