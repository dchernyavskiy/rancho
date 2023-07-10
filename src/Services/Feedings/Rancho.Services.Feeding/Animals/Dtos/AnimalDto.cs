
using Rancho.Services.Feeding.Animals.Enums;

namespace Rancho.Services.Feeding.Animals.Dtos;

public class AnimalDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string RfidTagId { get; set; } = null!;
}
