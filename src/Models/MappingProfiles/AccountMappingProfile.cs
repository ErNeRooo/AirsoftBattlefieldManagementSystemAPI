using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDto>();

            CreateMap<PostAccountDto, Account>()
                .ForMember(
                    destinationMember: dest => dest.AccountId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.PasswordHash,
                    memberOptions: opt => opt.MapFrom(a => a.Password));

            CreateMap<PutAccountDto, Account>()
                .ForMember(
                    destinationMember: dest => dest.AccountId,
                    memberOptions: opt => opt.Ignore())
                .ForMember(
                    destinationMember: dest => dest.PasswordHash,
                    memberOptions: opt => opt.MapFrom(a => a.Password));
        }
    }
}
