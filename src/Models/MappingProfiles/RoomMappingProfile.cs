using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDto>();
            
            CreateMap<Room, RoomWithRelatedEntitiesDto>()
                .ForMember(destinationMember: dest => dest.Players, 
                    source => source.MapFrom(src => src.Players))
                .ForMember(destinationMember: dest => dest.Teams, 
                    source => source.MapFrom(src => src.Teams))
                
                .ForMember(destinationMember: dest => dest.Kills, 
                    source => source.Condition(src => src.Battle is not null))
                .ForMember(destinationMember: dest => dest.Kills, 
                    source => source.MapFrom(src => src.Battle!.Kills))

                .ForMember(destinationMember: dest => dest.Deaths, 
                    source => source.Condition(src => src.Battle is not null))
                .ForMember(destinationMember: dest => dest.Deaths, 
                    source => source.MapFrom(src => src.Battle!.Deaths))
                
                .ForMember(destinationMember: dest => dest.Locations, 
                    source => source.Condition(src => src.Battle is not null))
                .ForMember(destinationMember: dest => dest.Locations, 
                    source => source.MapFrom(src => src.Battle!.PlayerLocations))
                
                .ForMember(destinationMember: dest => dest.MapPings, 
                    source => source.Condition(src => src.Battle is not null))
                .ForMember(destinationMember: dest => dest.MapPings, 
                    source => source.MapFrom(src => src.Battle!.MapPings))
                
                .ForMember(destinationMember: dest => dest.Orders, 
                    source => source.Condition(src => src.Battle is not null))
                .ForMember(destinationMember: dest => dest.Orders, 
                    source => source.MapFrom(src => src.Battle!.Orders))
                
                .ForMember(destinationMember: dest => dest.Zones, 
                    source => source.Condition(src => src.Battle is not null))
                .ForMember(destinationMember: dest => dest.Zones, 
                    source => source.MapFrom(src => src.Battle!.Zones))
                ;

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
                .ForMember(dest => dest.MaxPlayers, opt => opt.Condition(src => src.MaxPlayers != null))
                .ForMember(dest => dest.JoinCode, opt => opt.Condition(src => src.JoinCode != null))
                .ForMember(dest => dest.PasswordHash, opt => opt.Condition(src => src.Password != null))
                .ForMember(dest => dest.AdminPlayerId, opt => opt.Condition(src => src.AdminPlayerId != null));
        }
    }
}
