using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class ZoneVertexMappingProfile : Profile
    {
        public ZoneVertexMappingProfile()
        {
            CreateMap<ZoneVertex, ZoneVertexDto>();
            CreateMap<PostZoneVertexDto, ZoneVertex>();
        }
    }
}
