using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Topic;
using Galini.Models.Payload.Response.Topic;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class TopicMapper : Profile
    {
        public TopicMapper()
        {
            CreateMap<CreateTopicRequest, Topic>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.GetDescriptionFromEnum()))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Topic, CreateTopicResponse>();

            CreateMap<Topic, GetTopicResponse>();

            CreateMap<UpdateTopicRequest, Topic>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));
        }
    }
}
