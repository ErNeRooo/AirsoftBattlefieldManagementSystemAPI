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

            CreateMap<PostPlayerDto, Player>()
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

            CreateMap<PutPlayerDto, Player>()
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
                )
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.TeamId, opt => opt.Condition(src => src.TeamId != null))
                .ForMember(dest => dest.IsDead, opt => opt.Condition(src => src.IsDead != null));
        }
    }
}
