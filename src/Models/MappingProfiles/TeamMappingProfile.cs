using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>();

            CreateMap<PostTeamDto, Team>()
                .ForMember(
                    destinationMember: dest => dest.TeamId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Players, 
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Room,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutTeamDto, Team>()
                .ForMember(
                    destinationMember: dest => dest.TeamId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Players,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Room,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.RoomId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.OfficerPlayerId, opt => opt.Condition(src => src.OfficerPlayerId != null));
        }
    }
}
