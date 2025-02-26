using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.UserPresence;
using Galini.Models.Payload.Response.UserPresence;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class UserPresenceMapper : Profile
    {
        public UserPresenceMapper()
        {
            CreateMap<CreateUserPresenceRequest, UserPresence>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateUserPresenceRequest, UserPresence>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UserPresence, CreateUserPresenceResponse>();
        }
    }
}
