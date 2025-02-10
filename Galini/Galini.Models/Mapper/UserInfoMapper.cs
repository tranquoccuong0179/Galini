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

            CreateMap<UserInfo, CreateUserInfoResponse>();
        }
    }
}
