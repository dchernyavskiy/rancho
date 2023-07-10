using MassTransit;
using Microsoft.Extensions.Logging;
using Rancho.Services.Identification.Animals.Enums;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using SqlStreamStore.Logging;

namespace Rancho.Services.Identification.Animals.Features.CreatingAnimal.v1.Events.Integration.External;

public class AnimalCreatedConsumer : IConsumer<AnimalCreatedV1>
{
    private readonly IIdentificationDbContext _context;
    private readonly ILogger<AnimalCreatedConsumer> _logger;

    public AnimalCreatedConsumer(IIdentificationDbContext context, ILogger<AnimalCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AnimalCreatedV1> context)
    {
        try
        {
            _logger.LogInformation("In Animal Created Consumer.");
            var entity = new Animal(context.Message.Id)
                         {
                             Species = context.Message.Species,
                             BirthDate = context.Message.BirthDate,
                             Gender = Enum.Parse<Gender>(context.Message.Gender),
                             EarTagNumber = context.Message.EarTagNumber,
                             FarmId = context.Message.FarmId
                         };

            await _context.Animals.AddAsync(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Animal with id {entity.Id} was added to db");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
