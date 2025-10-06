using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class MapPingMappingProfile : Profile
    {
        public MapPingMappingProfile()
        {
            CreateMap<MapPing, MapPingDto>()
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

            CreateMap<PostMapPingDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutMapPingDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
                .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Latitude != null))
                .ForMember(dest => dest.Bearing, opt => opt.Condition(src => src.Bearing != null))
                .ForMember(dest => dest.Accuracy, opt => opt.Condition(src => src.Accuracy != null))
                .ForMember(dest => dest.Time, opt => opt.Condition(src => src.Time != null));

            CreateMap<MapPingDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PostMapPingDto, MapPing>()
                .ForMember(
                    destinationMember: dest => dest.MapPingId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutMapPingDto, MapPing>()
                .ForMember(
                    destinationMember: dest => dest.MapPingId,
                    memberOptions: opt => opt.Ignore());
        }
    }
}
