﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
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
                    source => source.MapFrom(src => src.Teams));

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
