using AutoMapper;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Models;

namespace Rancho.Services.Management.Works;

public class WorkMappers : Profile
{
    public WorkMappers()
    {
        CreateMap<Animal, AnimalDto>();
    }
}
