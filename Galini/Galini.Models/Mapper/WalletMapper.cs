using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Response.Wallet;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class WalletMapper : Profile
    {
        public WalletMapper()
        {
            CreateMap<Account, Wallet>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Wallet, GetWalletResponse>();
        }
    }
}
