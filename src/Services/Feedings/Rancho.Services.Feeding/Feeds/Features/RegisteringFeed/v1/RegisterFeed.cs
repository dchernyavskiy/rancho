using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Feeds.ValueObjects;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Feeds.Features.RegisteringFeed.v1;

public record RegisterFeed() : ICreateCommand<RegisterFeedResponse>, IMapWith<Feed>
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Nutrition Nutrition { get; set; }
    public decimal WeightInStock { get; set; }
    public Guid FarmId { get; set; }
}

public class RegisterFeedValidator : AbstractValidator<RegisterFeed>
{
    public RegisterFeedValidator()
    {
    }
}

public class RegisterFeedHandler : ICommandHandler<RegisterFeed, RegisterFeedResponse>
{
    private readonly IFeedingDbContext _context;
    private readonly IMapper _mapper;

    public RegisterFeedHandler(IFeedingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegisterFeedResponse> Handle(RegisterFeed request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Feed>(request);

        await _context.Feeds.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterFeedResponse(entity);
    }
}

public record RegisterFeedResponse(Feed Feed);

public class CreateAnimalEndpoint : EndpointBaseAsync.WithRequest<RegisterFeed>.WithActionResult<RegisterFeedResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateAnimalEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(FeedConfigs.FeedsPrefixUri + "/register", Name = "RegisterFeed")]
    [ProducesResponseType(typeof(RegisterFeedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Register Feed",
        Description = "Register Feed",
        OperationId = "RegisterFeed",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<ActionResult<RegisterFeedResponse>> HandleAsync(
        [FromBody] RegisterFeed request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
