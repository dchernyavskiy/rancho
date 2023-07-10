using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Feeding.Animals.Features.DeletingAnimal.v1.Events.Integration.External;

public class AnimalDeletedConsumer : IConsumer<AnimalDeletedV1>
{
    private readonly IFeedingDbContext _context;
    private readonly ICommandProcessor _commandProcessor;

    public AnimalDeletedConsumer(IFeedingDbContext context, ICommandProcessor commandProcessor)
    {
        _context = context;
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<AnimalDeletedV1> context)
    {
        try
        {
            var entity = await _context.Animals.FirstOrDefaultAsync(x => x.Id == context.Message.Id);
            Guard.Against.Null(entity, "Entity was not found");

            _context.Animals.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
        }
    }
}
