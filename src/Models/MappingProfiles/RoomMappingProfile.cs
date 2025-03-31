using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDto>();

            CreateMap<PostRoomDto, Room>()
                .ForMember(
                    destinationMember: dest => dest.AdminPlayer,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.PasswordHash,
                    memberOptions: opt => opt.MapFrom(r => r.Password))
                .ForMember(
                    destinationMember: dest => dest.RoomId, 
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Teams,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Battles,
                    memberOptions: opt => opt.Ignore());

            CreateMap<PutRoomDto, Room>()
                .ForMember(
                    destinationMember: dest => dest.AdminPlayer,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.PasswordHash,
                    memberOptions: opt => opt.MapFrom(r => r.Password))
                .ForMember(
                    destinationMember: dest => dest.RoomId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Teams,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Battles,
                    memberOptions: opt => opt.Ignore())
                .ForMember(dest => dest.MaxPlayers, opt => opt.Condition(src => src.MaxPlayers != null))
                .ForMember(dest => dest.JoinCode, opt => opt.Condition(src => src.JoinCode != null))
                .ForMember(dest => dest.PasswordHash, opt => opt.Condition(src => src.Password != null))
                .ForMember(dest => dest.AdminPlayer, opt => opt.Condition(src => src.AdminPlayerId != null));
        }
    }
}
