using AutoMapper;
using BankingSystem.Dtos;
using BankingSystem.Models;

namespace BankingSystem.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Account, AccountReadDto>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                ;
        }
    }
}
