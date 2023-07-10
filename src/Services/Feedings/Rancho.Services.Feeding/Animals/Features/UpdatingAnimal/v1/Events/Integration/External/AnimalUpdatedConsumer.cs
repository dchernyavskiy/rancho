using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Animals.Enums;
using Rancho.Services.Feeding.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Feeding.Animals.Features.UpdatingAnimal.v1.Events.Integration.External;

public class AnimalUpdatedConsumer : IConsumer<AnimalUpdatedV1>
{
    private readonly IFeedingDbContext _context;

    public AnimalUpdatedConsumer(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<AnimalUpdatedV1> context)
    {
        try
        {
            var entity = await _context.Animals
                             .FirstOrDefaultAsync(x => x.Id == context.Message.Id);

            Guard.Against.Null(entity, nameof(entity));

            entity.Species = context.Message.Species;
            entity.BirthDate = context.Message.BirthDate;
            entity.Gender = Enum.Parse<Gender>(context.Message.Gender);
            entity.EarTagNumber = context.Message.EarTagNumber;
            entity.FarmId = context.Message.FarmId;

            _context.Animals.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {
        }
    }
}
