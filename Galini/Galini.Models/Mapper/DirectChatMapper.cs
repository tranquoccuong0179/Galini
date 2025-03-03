using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.DirectChat;
using Galini.Models.Payload.Response.DirectChat;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class DirectChatMapper : Profile
    {
        public DirectChatMapper()
        {
            CreateMap<CreateDirectChatRequest, DirectChat>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<DirectChat, CreateDirectChatResponse>();
        }
    }
}
