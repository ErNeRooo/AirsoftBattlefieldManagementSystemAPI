﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
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
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive != null));
        }
    }
}
