using System.Linq.Dynamic.Core;
using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals;
using Rancho.Services.Management.Farmers.Dtos;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farmers.Features.GettingListOfFarmers.v1;

public record GetListOfFarmers() : CListQuery<GetListOfFarmersResponse>;

class GetListOfFarmersHandler : IQueryHandler<GetListOfFarmers, GetListOfFarmersResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetListOfFarmersHandler(
        IManagementDbContext context,
        ISecurityContextAccessor securityContextAccessor,
        IMapper mapper
    )
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
        _mapper = mapper;
    }

    public async Task<GetListOfFarmersResponse> Handle(GetListOfFarmers request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);
        var query = _context.Farmers
            .Include(x => x.Farm)
            .Where(x => x.Farm.OwnerId == userId);

        foreach (var filter in request.Filters)
        {
            query = query.Where(filter);
        }

        foreach (var sort in request.Sorts)
        {
            query = query.OrderBy(sort);
        }

        var farmers = await query.ProjectTo<FarmerDto>(_mapper.ConfigurationProvider)
                          .ApplyPagingAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);
        // var farmers = await _context.Farmers
        //                   .Include(x => x.Farm)
        //                   .Where(x => x.Farm.OwnerId == userId)
        //                   .Filter(request.Filters)
        //                   .Sort(request.Sorts)
        //                   .ProjectTo<FarmerDto>(_mapper.ConfigurationProvider)
        //                   .ApplyPagingAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);

        return new GetListOfFarmersResponse(farmers);
    }
}

public record GetListOfFarmersResponse(ListResultModel<FarmerDto> Body);

public class GetListOfFarmersEndpoint : EndpointBaseAsync.WithRequest<GetListOfFarmers>.WithActionResult
    <GetListOfFarmersResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetListOfFarmersEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FarmerConfigs.FarmersPrefixUri + "/get-all", Name = "GetFarmers")]
    [ProducesResponseType(typeof(GetListOfFarmersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting All Farmers",
        Description = "Getting All Farmers",
        OperationId = "GetFarmers",
        Tags = new[]
               {
                   FarmerConfigs.Tag
               })]
    public override async Task<ActionResult<GetListOfFarmersResponse>> HandleAsync(
        [FromQuery] GetListOfFarmers request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
