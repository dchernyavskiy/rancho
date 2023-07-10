using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Tags.Features.DetachingRfidTag.v1;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Identification.Animals.Features.DeletingAnimal.v1.Events.Integration.External;

public class AnimalDeletedConsumer : IConsumer<AnimalDeletedV1>
{
    private readonly IIdentificationDbContext _context;
    private readonly ICommandProcessor _commandProcessor;

    public AnimalDeletedConsumer(IIdentificationDbContext context, ICommandProcessor commandProcessor)
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

            await _commandProcessor.SendAsync(new DetachRfidTag() {RfidTagId = (Guid)entity.RfidTagId!});
        }
        catch (Exception) { }
    }
}
