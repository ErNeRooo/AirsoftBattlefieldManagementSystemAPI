using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDto>();

            CreateMap<CreateRoomDto, Room>()
                .ForMember(
                    destinationMember: dest => dest.RoomId, 
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Teams,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Battles,
                    memberOptions: opt => opt.Ignore());

            CreateMap<UpdateRoomDto, Room>()
                .ForMember(
                    destinationMember: dest => dest.RoomId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Teams,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.Battles,
                    memberOptions: opt => opt.Ignore());
        }
    }
}
