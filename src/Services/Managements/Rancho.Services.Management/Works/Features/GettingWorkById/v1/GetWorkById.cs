using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Management.Works.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Works.Features.GettingWorkById.v1;

public record GetWorkById(Guid Id) : IQuery<GetWorkByIdResponse>;

public class GetWorkByIdHandler : IQueryHandler<GetWorkById, GetWorkByIdResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkByIdHandler(IManagementDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetWorkByIdResponse> Handle(GetWorkById request, CancellationToken cancellationToken)
    {
        var entity = await _context.Works
                         .FirstOrDefaultAsync(
                             x => x.Id == request.Id,
                             cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Animal was not found.");

        var entityDto = _mapper.Map<WorkDto>(entity);

        return new GetWorkByIdResponse(entityDto);
    }
}

public class GetWorkByIdEndpoint : EndpointBaseAsync.WithRequest<GetWorkById>.WithResult<GetWorkByIdResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetWorkByIdEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(WorkConfigs.WorksPrefixUri, Name = "GetWorkById")]
    [ProducesResponseType(typeof(GetWorkByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Work By Id",
        Description = "Get Work By Id",
        OperationId = "GetWorkById",
        Tags = new[]
               {
                   WorkConfigs.Tag
               })]
    public override async Task<GetWorkByIdResponse> HandleAsync(
        [FromQuery] GetWorkById request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _queryProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
