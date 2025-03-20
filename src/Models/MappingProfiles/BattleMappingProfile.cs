using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class BattleMappingProfile : Profile
    {
        public BattleMappingProfile()
        {
            CreateMap<Battle, BattleDto>();

            CreateMap<PostBattleDto, Battle>().ForMember(
                    destinationMember: dest => dest.BattleId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Room,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutBattleDto, Battle>()
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
