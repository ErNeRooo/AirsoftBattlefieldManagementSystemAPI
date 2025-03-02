using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class BattleMappingProfile : Profile
    {
        public BattleMappingProfile()
        {
            CreateMap<Battle, BattleDto>();

            CreateMap<CreateBattleDto, Battle>().ForMember(
                    destinationMember: dest => dest.BattleId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Room,
                    memberOptions: opt => opt.Ignore());

            CreateMap<UpdateBattleDto, Battle>()
                .ForMember(
                    destinationMember: dest => dest.BattleId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Room,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.RoomId,
                    memberOptions: opt => opt.Ignore()); ;
        }
    }
}
