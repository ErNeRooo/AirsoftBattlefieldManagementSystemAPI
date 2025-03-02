using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>();

            CreateMap<CreateTeamDto, Team>()
                .ForMember(
                    destinationMember: dest => dest.TeamId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Players, 
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Room,
                    memberOptions: opt => opt.Ignore());

            CreateMap<UpdateTeamDto, Team>()
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
                    memberOptions: opt => opt.Ignore());
        }
    }
}
