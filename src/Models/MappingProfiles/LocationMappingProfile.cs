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
                    memberOptions: opt => opt.Ignore());
        }
    }
}
