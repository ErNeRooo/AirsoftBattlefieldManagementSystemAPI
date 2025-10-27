using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class ZoneMappingProfile : Profile
    {
        public ZoneMappingProfile()
        {
            CreateMap<Zone, ZoneDto>();
            
            CreateMap<PutZoneDto, Zone>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.Type, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Type)));
            
            CreateMap<PostZoneDto, Zone>();
        }
    }
}
