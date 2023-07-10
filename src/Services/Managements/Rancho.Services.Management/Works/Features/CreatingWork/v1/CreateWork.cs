using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Management.Works.Dtos;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Works.Features.CreatingWork.v1;

public record CreateWork(
    string Name,
    string Description,
    DateTime Start,
    DateTime End,
    Guid FarmerId
) : ITxCreateCommand<CreateWorkResponse>;

public class CreateWorkValidator : AbstractValidator<CreateWork>
{
    public CreateWorkValidator()
    {
        CascadeMode = CascadeMode.Stop;
    }
}

public class CreateWorkHandler : ICommandHandler<CreateWork, CreateWorkResponse>
{
    private readonly ILogger<CreateWorkHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IManagementDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public CreateWorkHandler(
        ILogger<CreateWorkHandler> logger,
        IMapper mapper,
        IManagementDbContext context,
        ISecurityContextAccessor securityContextAccessor
    )
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<CreateWorkResponse> Handle(CreateWork request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var farmer = await _context.Farmers
                         .Include(x => x.Farm)
                         .FirstOrDefaultAsync(x => x.Id == request.FarmerId, cancellationToken: cancellationToken);

        Guard.Against.Null(farmer, nameof(farmer));

        var entity = new Work()
                     {
                         Name = request.Name,
                         Description = request.Description,
                         Start = request.Start,
                         End = request.End,
                         FarmerId = request.FarmerId
                     };

        await _context.Works.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Work with Id: {entity.Id} was created");

        var entityDto = _mapper.Map<WorkDto>(entity);

        return new CreateWorkResponse(entityDto);
    }
}

public record CreateWorkResponse(WorkDto Work);
