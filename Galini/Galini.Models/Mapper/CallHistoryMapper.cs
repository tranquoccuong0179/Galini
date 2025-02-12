using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.CallHistory;
using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Response.CallHistory;
using Galini.Models.Payload.Response.UserCall;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class CallHistoryMapper : Profile
    {
        public CallHistoryMapper()
        {
            CreateMap<CreateCallHistoryRequest, CallHistory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateCallHistoryRequest, CallHistory>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.TimeStart, opt => opt.Condition(src => src.TimeStart != default))
                .ForMember(dest => dest.TimeEnd, opt => opt.Condition(src => src.TimeEnd != default))
                .ForMember(dest => dest.Duration, opt => opt.Condition(src => src.Duration != 0));

            CreateMap<CallHistory, CreateCallHistoryResponse>();
        }
    }
}
