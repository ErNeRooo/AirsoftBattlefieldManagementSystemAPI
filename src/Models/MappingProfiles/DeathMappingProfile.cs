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

            CreateMap<PostDeathDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutDeathDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<DeathDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PostDeathDto, Death>()
                .ForMember(
                    destinationMember: dest => dest.DeathId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutDeathDto, Death>()
                .ForMember(
                    destinationMember: dest => dest.DeathId,
                    memberOptions: opt => opt.Ignore());
        }
    }
}
