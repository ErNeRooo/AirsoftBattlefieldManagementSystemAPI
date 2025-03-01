using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerDto, Player>();
        }
    }
}
