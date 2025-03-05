using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerDto>();

            CreateMap<CreatePlayerDto, Player>()
                .ForMember(
                    destinationMember: dest => dest.PlayerId,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Account,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.AccountId,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Team,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Kills,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Deaths,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.PlayerLocations,
                    memberOptions: opt => opt.Ignore()
                );

            CreateMap<UpdatePlayerDto, Player>()
                .ForMember(
                    destinationMember: dest => dest.PlayerId,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Account,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.AccountId,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Team,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Kills,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.Deaths,
                    memberOptions: opt => opt.Ignore()
                )
                .ForMember(
                    destinationMember: dest => dest.PlayerLocations,
                    memberOptions: opt => opt.Ignore()
                );
        }
    }
}
