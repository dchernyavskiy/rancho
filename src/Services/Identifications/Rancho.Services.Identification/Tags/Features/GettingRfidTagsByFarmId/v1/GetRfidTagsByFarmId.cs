using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Tags.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identification.Tags.Features.GettingRfidTagsByFarmId.v1;

public record GetRfidTagsByFarmId(Guid FarmId) : IQuery<GetRfidTagsByFarmIdResponse>;

public record GetRfidTagsByFarmIdResponse(ICollection<RfidTag> rfidTags);

public class GetRfidTagsByFarmIdHandler : IQueryHandler<GetRfidTagsByFarmId, GetRfidTagsByFarmIdResponse>
{
    private readonly IIdentificationDbContext _context;

    public GetRfidTagsByFarmIdHandler(IIdentificationDbContext context)
    {
        _context = context;
    }

    public async Task<GetRfidTagsByFarmIdResponse> Handle(
        GetRfidTagsByFarmId request,
        CancellationToken cancellationToken
    )
    {
        var farm = await _context.Farms
                       .Include(x => x.RfidTags)
                       .FirstOrDefaultAsync(x => x.Id == request.FarmId, cancellationToken: cancellationToken);

        var tags = farm?.RfidTags ?? Enumerable.Empty<RfidTag>().ToList();

        return new(tags);
    }
}

public class GetRfidTagsByFarmIdEndpoint : EndpointBaseAsync.WithRequest<GetRfidTagsByFarmId>.WithoutResult
{
    private readonly IQueryProcessor _queryProcessor;

    public GetRfidTagsByFarmIdEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(RfidTagConfig.TagsPrefixUri + "/get-all-by-farm-id", Name = "GetRfidTagsByFarmId")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Attaching Rfid Tag",
        Description = "Attaching Rfid Tag",
        OperationId = "GetRfidTagsByFarmId",
        Tags = new[]
               {
                   RfidTagConfig.Tag
               })]
    public override async Task<ActionResult<GetRfidTagsByFarmIdResponse>> HandleAsync(
        [FromQuery] GetRfidTagsByFarmId request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
