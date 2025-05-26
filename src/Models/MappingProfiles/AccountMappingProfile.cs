using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
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
                    memberOptions: opt => opt.MapFrom(a => a.Password))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
                .ForMember(dest => dest.PasswordHash, opt => opt.Condition(src => src.Password != null));
        }
    }
}
