using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class KillMappingProfile : Profile
    {
        public KillMappingProfile()
        {
            CreateMap<Kill, KillDto>()
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

            CreateMap<PostKillDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutKillDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<KillDto, Location>()
                .ForMember(
                    destinationMember: dest => dest.LocationId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PostKillDto, Kill>()
                .ForMember(
                    destinationMember: dest => dest.KillId,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutKillDto, Kill>()
                .ForMember(
                    destinationMember: dest => dest.KillId,
                    memberOptions: opt => opt.Ignore());
        }
    }
}
