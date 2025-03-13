using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response.UserInfo;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class UserInfoMapper : Profile
    {
        public UserInfoMapper()
        {
            CreateMap<CreateUserInfoRequest, UserInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateUserInfoRequest, UserInfo>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.DateStart, opt => opt.Condition(src => src.DateStart != default))
                .ForMember(dest => dest.DateEnd, opt => opt.Condition(src => src.DateEnd != default));

            CreateMap<UserInfo, CreateUserInfoResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Premium.Type))
                .ForMember(dest => dest.Friend, opt => opt.MapFrom(src => src.Premium.Friend))
                .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.Premium.Timelimit))
                .ForMember(dest => dest.Match, opt => opt.MapFrom(src => src.Premium.Match))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Premium.Price))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Account.Role))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Account.DateOfBirth))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Account.Gender))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Account.AvatarUrl));
        }
    }
}
