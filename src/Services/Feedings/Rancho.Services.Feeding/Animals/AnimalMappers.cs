using AutoMapper;
using Rancho.Services.Feeding.Animals.Dtos;
using Rancho.Services.Feeding.Animals.Models;

namespace Rancho.Services.Feeding.Animals;

public class AnimalMappers : Profile
{
    public AnimalMappers()
    {
        CreateMap<Animal, AnimalDto>();
    }
}
