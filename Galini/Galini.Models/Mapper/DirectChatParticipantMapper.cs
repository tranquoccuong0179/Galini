using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.DirectChatParticipant;
using Galini.Models.Payload.Response.DirectChatParticipant;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class DirectChatParticipantMapper : Profile
    {
        public DirectChatParticipantMapper()
        {
            CreateMap<CreateDirectChatParticipant, DirectChatParticipant>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<DirectChatParticipant, CreateDirectChatParticipantResponse>();
            CreateMap<DirectChatParticipant, GetDirectChatParticipantResponse>();
        }
    }
}
