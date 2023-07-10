using BuildingBlocks.Core.Domain;
using Rancho.Services.Identification.Animals.Enums;
using Rancho.Services.Identification.Farms.Models;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Animals.Models;

public class Animal : Aggregate<Guid>
{
    public Animal() : this(Guid.NewGuid()) { }

    public Animal(Guid id)
    {
        Id = id;
    }

    public string Species { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string EarTagNumber { get; set; } = null!;

    public Guid FarmId { get; set; }
    public Farm Farm { get; set; }

    public Guid? RfidTagId { get; set; }
    public RfidTag RfidTag { get; set; } = null!;
}
