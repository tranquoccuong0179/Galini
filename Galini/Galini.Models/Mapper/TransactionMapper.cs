using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Response.Transaction;

namespace Galini.Models.Mapper
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<Transaction, GetTransactionResponse>();
            CreateMap<Transaction, GetTransactionAdminResponse>()
                .ForMember(dest => dest.GetAccountResponse, opt => opt.MapFrom(src => src.Wallet.Account));
        }
    }
}
