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
                    memberOptions: opt => opt.Ignore())
                .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
                .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Latitude != null))
                .ForMember(dest => dest.Bearing, opt => opt.Condition(src => src.Bearing != null))
                .ForMember(dest => dest.Accuracy, opt => opt.Condition(src => src.Accuracy != null))
                .ForMember(dest => dest.Time, opt => opt.Condition(src => src.Time != null));

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
