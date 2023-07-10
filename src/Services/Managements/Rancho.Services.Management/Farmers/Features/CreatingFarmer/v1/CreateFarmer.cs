using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farmers.Features.CreatingFarmer.v1;

public record CreateFarmer() : ICreateCommand<CreateFarmerResponse>, IMapWith<Farmer>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public Guid FarmId { get; set; }
}

public class CreateFarmerValidator : AbstractValidator<CreateFarmer>
{
    public CreateFarmerValidator()
    {
        RuleFor(x => x.FarmId).NotNull().NotEmpty();
    }
}

public class CreateFarmerHandler : ICommandHandler<CreateFarmer, CreateFarmerResponse>
{
    private readonly IManagementDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IMapper _mapper;
    private readonly IMessagePersistenceService _messagePersistenceService;

    public CreateFarmerHandler(
        IManagementDbContext context,
        ISecurityContextAccessor securityContextAccessor,
        IMapper mapper,
        IMessagePersistenceService messagePersistenceService
    )
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
        _mapper = mapper;
        _messagePersistenceService = messagePersistenceService;
    }

    public async Task<CreateFarmerResponse> Handle(CreateFarmer request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(_securityContextAccessor.UserId);

        var userId = Guid.Parse(_securityContextAccessor.UserId);
        var farm = await _context.Farms
                       .Include(x => x.Farmers)
                       .SingleAsync(
                           x => x.Id == request.FarmId && x.OwnerId == userId,
                           cancellationToken: cancellationToken);

        Guard.Against.Null(farm);

        var farmer = _mapper.Map<Farmer>(request);

        await _context.Farmers.AddAsync(farmer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _messagePersistenceService.AddPublishMessageAsync(
            new MessageEnvelope<FarmerCreatedV1>(
                new FarmerCreatedV1(
                    farmer.Id,
                    farmer.FirstName,
                    farmer.LastName,
                    farmer.Email,
                    farmer.PhoneNumber),
                new Dictionary<string, object?>()),
            cancellationToken);

        return new CreateFarmerResponse(farmer);
    }
}

public record CreateFarmerResponse(Farmer Farmer);

public class CreateFarmerEndpoint : EndpointBaseAsync.WithRequest<CreateFarmer>.WithActionResult<CreateFarmerResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateFarmerEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(FarmerConfigs.FarmersPrefixUri, Name = "CreateFarmer")]
    [ProducesResponseType(typeof(CreateFarmerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Create Farmer",
        Description = "Create Farmer",
        OperationId = "CreateFarmer",
        Tags = new[]
               {
                   FarmerConfigs.Tag
               })]
    public override async Task<ActionResult<CreateFarmerResponse>> HandleAsync(
        [FromBody] CreateFarmer request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
