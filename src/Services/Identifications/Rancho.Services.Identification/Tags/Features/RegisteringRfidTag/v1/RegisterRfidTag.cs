using BuildingBlocks.Abstractions.CQRS.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Tags.Enums;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Tags.Features.RegisteringRfidTag.v1;

public record RegisterRfidTag() : ICreateCommand<RegisterRfidTagResponse>
{
    public string EarTagNumber { get; set; }
    public Guid FarmId { get; set; }
}

public class RegisterRfidTagValidator : AbstractValidator<RegisterRfidTag>
{
    public RegisterRfidTagValidator()
    {
        CascadeMode = CascadeMode.Stop;
    }
}

public class RegisterRfidTagHandler : ICommandHandler<RegisterRfidTag, RegisterRfidTagResponse>
{
    private readonly IIdentificationDbContext _context;

    public RegisterRfidTagHandler(IIdentificationDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterRfidTagResponse> Handle(RegisterRfidTag request, CancellationToken cancellationToken)
    {
        var animal = await _context.Animals
                         .FirstOrDefaultAsync(
                             x => x.EarTagNumber == request.EarTagNumber && x.FarmId == request.FarmId,
                             cancellationToken: cancellationToken);

        var tag = animal switch
                  {
                      null => new RfidTag() {Status = Status.Unused, FarmId = request.FarmId},
                      _ => new RfidTag() {Status = Status.Attached, AnimalId = animal.Id, FarmId = request.FarmId}
                  };
        if (animal != null)
        {
            animal.RfidTagId = tag.Id;
            _context.Animals.Update(animal);
        }

        await _context.RfidTags.AddAsync(tag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterRfidTagResponse(tag);
    }
}

public record RegisterRfidTagResponse(RfidTag Tag);
