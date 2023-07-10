using AutoMapper;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Abstractions.Mapping;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Models;

namespace Rancho.Services.Management.Animals.Dtos;

public class AnimalDto : IMapWith<Animal>, IHaveIdentity<Guid>
{
    public Guid Id { get; set; }
    public string Species { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string EarTagNumber { get; set; } = null!;
    public Guid FarmId { get; set; }
}
