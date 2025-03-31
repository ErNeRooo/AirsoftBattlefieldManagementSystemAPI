using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<PostLocationDto, PlayerLocation>().ForMember(
                destinationMember: dest => dest.LocationId,
                memberOptions: opt => opt.Ignore());

            CreateMap<PostLocationDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutLocationDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
                .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Latitude != null))
                .ForMember(dest => dest.Bearing, opt => opt.Condition(src => src.Bearing != null))
                .ForMember(dest => dest.Accuracy, opt => opt.Condition(src => src.Accuracy != null))
                .ForMember(dest => dest.Time, opt => opt.Condition(src => src.Time != null));
        }
    }
}
