



using AutoMapper;
using Microsoft.VisualStudio.Services.Account;
using Technovert.WebApi.DTOs.AccountDTOs;
using Technovert.WebApi.DTOs.BankDTO;
using Technovert.WebApi.DTOs.TransactionDTOs;
using TechonovertAtm.Models;


namespace Technovert.WebApi.AutoMapper
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper()
        {
            CreateMap<GetAccountDTO, BankAccount>().ReverseMap();
            CreateMap<BankAccount, PostAccountDTO>().ReverseMap();
            CreateMap<DeleteAccountDTO, BankAccount>().ReverseMap();
            CreateMap<BalanceDTO, BankAccount>().ReverseMap();
            CreateMap<PutAccountDTO, BankAccount>().ReverseMap();
            CreateMap<BankDTO, Bank>().ReverseMap();
            CreateMap<CreateTransactionDTO, Transaction>().ReverseMap();
            CreateMap<GetTransactionDTO, Transaction>().ReverseMap();
            CreateMap<DefaultTransactionDTO, Transaction>().ReverseMap();
        }
    }
}