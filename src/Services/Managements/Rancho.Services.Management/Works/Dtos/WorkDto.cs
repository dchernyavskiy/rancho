using AutoMapper;
using BuildingBlocks.Abstractions.Mapping;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Farmers.Dtos;
using Rancho.Services.Management.Works.Enums;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Works.Dtos;

public class WorkDto : IMapWith<Work>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Status { get; set; }
    public Guid FarmerId { get; set; }
    public FarmerDto? Farmer { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Work, WorkDto>()
            .ForMember(x => x.Status, opts => opts.MapFrom(src => src.Status.ToString()))
            .ReverseMap()
            .ForMember(x => x.Status, opts => opts.MapFrom(src => Enum.Parse<WorkStatus>(src.Status)));
    }
}
