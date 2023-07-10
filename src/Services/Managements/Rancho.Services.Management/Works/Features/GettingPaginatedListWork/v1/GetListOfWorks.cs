using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
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
using Microsoft.IdentityModel.Tokens;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Management.Works.Dtos;
using Rancho.Services.Management.Works.Enums;
using Rancho.Services.Management.Works.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Works.Features.GettingPaginatedList.v1;

public record GetListOfWorks : CListQuery<GetListOfWorksResponse>;

public class GetListOfWorksHandler : IQueryHandler<GetListOfWorks, GetListOfWorksResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetListOfWorksHandler(
        IManagementDbContext context,
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor
    )
    {
        _context = context;
        _mapper = mapper;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<GetListOfWorksResponse> Handle(GetListOfWorks request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(_securityContextAccessor.UserId, nameof(_securityContextAccessor.UserId));

        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var role = _securityContextAccessor.Role;

        IQueryable<Work> works = default!;

        if (role == ManagementConstants.Role.Admin)
        {
            works = _context.Farms
                .Include(x => x.Farmers)
                .ThenInclude(x => x.Works)
                .ThenInclude(x => x.Farmer)
                .SelectMany(x => x.Farmers.SelectMany(x => x.Works));
        }
        else if (role == ManagementConstants.Role.Farmer || role == ManagementConstants.Role.User)
        {
            works = _context.Works.Where(x => x.FarmerId == userId).Where(x => x.Status != WorkStatus.Done);
        }
        else
        {
            return new GetListOfWorksResponse(ListResultModel<WorkDto>.Empty);
        }

        return new GetListOfWorksResponse(
            await works
                .Filter(request.Filters)
                .Sort(request.Sorts)
                .ProjectTo<WorkDto>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize, cancellationToken: cancellationToken));
    }
}

public record GetListOfWorksResponse(ListResultModel<WorkDto> Body);

public class GetListOfWorksEndpoint : EndpointBaseAsync.WithRequest<GetListOfWorks>.WithResult<GetListOfWorksResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetListOfWorksEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(WorkConfigs.WorksPrefixUri + "/get-all", Name = "GetWorks")]
    [ProducesResponseType(typeof(GetListOfWorksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Work",
        Description = "Get Work",
        OperationId = "GetWorks",
        Tags = new[]
               {
                   WorkConfigs.Tag
               })]
    public override async Task<GetListOfWorksResponse> HandleAsync(
        [FromQuery] GetListOfWorks request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _queryProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
