using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.ListenerInfo;
using Galini.Models.Payload.Response.ListenerInfo;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class ListenerInforMapper : Profile
    {
        public ListenerInforMapper()
        {
            CreateMap<CreateListenerInfoRequest, ListenerInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Star, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<ListenerInfo, CreateListenerInfoResponse>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account));
        }
    }
}
