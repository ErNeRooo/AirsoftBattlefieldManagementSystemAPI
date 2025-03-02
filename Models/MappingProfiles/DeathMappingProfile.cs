using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class DeathMappingProfile : Profile
    {
        public DeathMappingProfile()
        {
            CreateMap<Death, DeathDto>()
                .ForMember(destinationMember: dest => dest.Longitude,
                    memberOptions: opt =>
                        opt.MapFrom(src => src.Location.Longitude))

                .ForMember(destinationMember: dest => dest.Latitude,
                    memberOptions: opt =>
                        opt.MapFrom(src => src.Location.Latitude))

                .ForMember(destinationMember: dest => dest.Accuracy,
                        memberOptions: opt => 
                        opt.MapFrom(src => src.Location.Accuracy))

                .ForMember(destinationMember: dest => dest.Bearing,
                    memberOptions: opt => 
                        opt.MapFrom(src => src.Location.Bearing))

                .ForMember(destinationMember: dest => dest.Time,
                    memberOptions: opt =>
                        opt.MapFrom(src => src.Location.Time));

            CreateMap<CreateDeathDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<UpdateDeathDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<DeathDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<CreateDeathDto, Death>()
                .ForMember(
                    destinationMember: dest => dest.DeathId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<UpdateDeathDto, Death>()
                .ForMember(
                    destinationMember: dest => dest.DeathId,
                    memberOptions: opt => opt.Ignore());
        }
    }
}
