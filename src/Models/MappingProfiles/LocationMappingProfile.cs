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
            CreateMap<Location, LocationDto>().ForMember(destinationMember: dest => dest.PlayerId,
                memberOptions: opt => opt.MapFrom(
                    (src, dest, destMember, context) =>
                    {
                        int? id = context.Items["playerId"] as int?;
                        return id != null ? id : throw new Exception("PlayerId is required in mapping");
                    }
                    ));

            CreateMap<CreateLocationDto, PlayerLocation>().ForMember(
                destinationMember: dest => dest.LocationId,
                memberOptions: opt => opt.Ignore());

            CreateMap<CreateLocationDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<UpdateLocationDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());
        }
    }
}
