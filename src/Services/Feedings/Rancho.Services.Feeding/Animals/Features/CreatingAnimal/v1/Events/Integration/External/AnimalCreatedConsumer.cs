using MassTransit;
using Rancho.Services.Feeding.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Rancho.Services.Feeding.Animals.Enums;
using Rancho.Services.Feeding.Animals.Models;

namespace Rancho.Services.Feeding.Animals.Features.CreatingAnimal.v1.Events.Integration.External;

public class AnimalCreatedConsumer : IConsumer<AnimalCreatedV1>
{
    private readonly IFeedingDbContext _context;

    public AnimalCreatedConsumer(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<AnimalCreatedV1> context)
    {
        try
        {
            var entity = new Animal()
                         {
                             BirthDate = context.Message.BirthDate,
                             Species = context.Message.Species,
                             Gender = Enum.Parse<Gender>(context.Message.Gender),
                             EarTagNumber = context.Message.EarTagNumber,
                             FarmId = context.Message.FarmId
                         };
            await _context.Animals.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {

        }
    }
}
