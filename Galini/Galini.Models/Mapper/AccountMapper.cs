using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Request.User;
using Galini.Models.Response.Account;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper() 
        {
            CreateMap<RegisterUserRequest, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordUtil.HashPassword(src.Password)))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RoleEnum.Customer.GetDescriptionFromEnum()))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.GetDescriptionFromEnum()))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Account, RegisterUserResponse>();
        }
    }
}
