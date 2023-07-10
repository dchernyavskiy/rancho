using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Bogus;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Feeds;
using Rancho.Services.Feeding.Feeds.Data;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Seeds;

public record SeedFeed(Guid FarmId) : ICommand<SeedFeedResponse>;

public class SeedFeedHandler : ICommandHandler<SeedFeed, SeedFeedResponse>
{
    private readonly IFeedingDbContext _context;
    private readonly ILogger<SeedFeedHandler> _logger;

    public SeedFeedHandler(IFeedingDbContext context, ILogger<SeedFeedHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SeedFeedResponse> Handle(SeedFeed request, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        var feeds = await _context.Feeds
                        .ToListAsync(cancellationToken: cancellationToken);
        var animals = await _context.Animals
                          .ToListAsync(cancellationToken: cancellationToken);
        foreach (var feed in feeds)
        {
            foreach (var animal in animals)
            {
                var date1 = DateTime.Now.AddMonths(-1);
                var date2 = DateTime.Now;
                for (DateTime i = date1; i <= date2; i = i.AddDays(1))
                {
                    var weight = faker.Random.Decimal(2m, 4m);
                    var feedPlan = new FeedPlan()
                                   {
                                       WeightDispensed = weight,
                                       WeightEaten = faker.Random.Decimal(1m, weight),
                                       DispenseDate = i,
                                       FixationDate = i,
                                       Feed = feed,
                                       Animal = animal
                                   };
                    await _context.FeedPlans.AddAsync(feedPlan, cancellationToken);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        // if (request.FarmId != Guid.Empty)
        // {
        //     var farm = await _context.Farms.FirstOrDefaultAsync(
        //                    x => x.Id == request.FarmId,
        //                    cancellationToken: cancellationToken);
        //     await _context.Feeds.AddRangeAsync(
        //         new FeedSeeder.FeedSeedFaker(request.FarmId).Generate(faker.Random.Int(5, 10)),
        //         cancellationToken);
        //     await _context.SaveChangesAsync(cancellationToken);
        //     _logger.LogInformation($"Farm with id {request.FarmId} was seeded");
        //     return new SeedFeedResponse();
        // }
        //
        // if (await _context.Farms.AnyAsync(cancellationToken: cancellationToken))
        // {
        //     if (!await _context.Feeds.AnyAsync(cancellationToken: cancellationToken))
        //     {
        //         var farmIds = await _context.Farms.Select(x => x.Id).ToListAsync(cancellationToken: cancellationToken);
        //         foreach (var id in farmIds)
        //         {
        //             await _context.Feeds.AddRangeAsync(
        //                 new FeedSeeder.FeedSeedFaker(id).Generate(faker.Random.Int(2, 5)),
        //                 cancellationToken);
        //         }
        //
        //         await _context.SaveChangesAsync(cancellationToken);
        //         _logger.LogInformation("Feeds was seeded successfully.");
        //     }
        //
        //
        //     var farms = await _context.Farms
        //                     .Include(x => x.Feeds)
        //                     .Include(x => x.Animals)
        //                     .ToListAsync(cancellationToken: cancellationToken);
        //     var date1 = DateTime.Now.AddMonths(-1);
        //     var date2 = DateTime.Now;
        //     for (DateTime i = date1; i <= date2; i = i.AddDays(1))
        //     {
        //         foreach (var farm in farms)
        //         {
        //             foreach (var animal in farm.Animals)
        //             {
        //                 foreach (var feed in farm.Feeds)
        //                 {
        //                     var weight = faker.Random.Decimal(2m, 4m);
        //                     var feedPlan = new FeedPlan()
        //                                    {
        //                                        WeightDispensed = weight,
        //                                        WeightEaten = faker.Random.Decimal(1m, weight),
        //                                        DispenseDate = i,
        //                                        FixationDate = i,
        //                                        Feed = feed,
        //                                        Animal = animal
        //                                    };
        //                     await _context.FeedPlans.AddAsync(feedPlan, cancellationToken);
        //                 }
        //             }
        //         }
        //     }
        //
        //     await _context.SaveChangesAsync(cancellationToken);
        // }

        return new SeedFeedResponse();
    }
}

public record SeedFeedResponse();

public class SeedFeedEndpoint : EndpointBaseAsync.WithRequest<SeedFeed>.WithActionResult<SeedFeedResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public SeedFeedEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(FeedConfigs.FeedsPrefixUri + "/seed", Name = "SeedFeed")]
    [ProducesResponseType(typeof(SeedFeedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Replenish Feed",
        Description = "Replenish Feed",
        OperationId = "SeedFeed",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<ActionResult<SeedFeedResponse>> HandleAsync(
        [FromBody] SeedFeed request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
