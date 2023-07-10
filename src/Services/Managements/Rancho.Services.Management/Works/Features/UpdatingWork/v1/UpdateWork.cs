using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Management.Works.Enums;
using Rancho.Services.Management.Works.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Works.Features.UpdatingWork.v1;

public record UpdateWork : IUpdateCommand<Unit>, IMapWith<Work>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Guid FarmerId { get; set; }
    public WorkStatus Status { get; set; }
}

public record MarkAsDoneWork : IUpdateCommand<MarkAsDoneWorkResponse>, IMapWith<Work>
{
    public Guid Id { get; set; }
}

public record MarkAsDoneWorkResponse();

public class UpdateWorkValidator : AbstractValidator<UpdateWork>
{
    public UpdateWorkValidator()
    {
        RuleFor(x => x.Start).LessThan(x => x.End).WithMessage("End date must be greater than start.");
    }
}

public class UpdateWorkHandler : ICommandHandler<UpdateWork, Unit>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;


    public UpdateWorkHandler(IManagementDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateWork request, CancellationToken cancellationToken)
    {
        var entity = await _context.Works
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Work was not found.");

        entity = _mapper.Map<Work>(request);
        _context.Works.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class MarkAsDoneWorkHandler : ICommandHandler<MarkAsDoneWork, MarkAsDoneWorkResponse>
{
    private readonly IManagementDbContext _context;
    private readonly IMapper _mapper;


    public MarkAsDoneWorkHandler(IManagementDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MarkAsDoneWorkResponse> Handle(MarkAsDoneWork request, CancellationToken cancellationToken)
    {
        var entity = await _context.Works
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Work was not found.");

        entity.Status = WorkStatus.Done;
        _context.Works.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new MarkAsDoneWorkResponse();
    }
}

public class UpdateWorkEndpoint : EndpointBaseAsync.WithRequest<UpdateWork>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateWorkEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(WorkConfigs.WorksPrefixUri, Name = "UpdateWork")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Create Work",
        Description = "Create Work",
        OperationId = "UpdateWork",
        Tags = new[]
               {
                   WorkConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] UpdateWork request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}

public class
    MarkAsDoneWorkEndpoint : EndpointBaseAsync.WithRequest<MarkAsDoneWork>.WithActionResult<MarkAsDoneWorkResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public MarkAsDoneWorkEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(WorkConfigs.WorksPrefixUri + "/mark-as-done", Name = "MarkAsDoneWork")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Mark Work As Done",
        Description = "Mark Work As Done",
        OperationId = "MarkAsDoneWork",
        Tags = new[]
               {
                   WorkConfigs.Tag
               })]
    public override async Task<ActionResult<MarkAsDoneWorkResponse>> HandleAsync(
        [FromBody] MarkAsDoneWork request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
