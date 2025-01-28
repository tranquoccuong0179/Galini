using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Response.UserCall;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class UserCallMapper : Profile
    {
        public UserCallMapper()
        {
            CreateMap<CreateUserCallRequest, UserCall>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CallRole, opt => opt.MapFrom(src => src.CallRole.GetDescriptionFromEnum()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateUserCallRequest, UserInfo>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UserCall, CreateUserCallResponse>();
        }
    }
}
