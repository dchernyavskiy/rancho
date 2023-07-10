
using BuildingBlocks.Abstractions.Mapping;
using Rancho.Services.Identification.Animals.Enums;
using Rancho.Services.Identification.Animals.Models;

namespace Rancho.Services.Identification.Animals.Dtos;

public class AnimalDto : IMapWith<Animal>
{
    public Guid Id { get; set; }
    public string Species { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string EarTagNumber { get; set; } = null!;
    public Guid RfidTagId { get; set; }
}
