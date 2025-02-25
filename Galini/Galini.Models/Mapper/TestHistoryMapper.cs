using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Review;
using Galini.Models.Payload.Request.TestHistory;
using Galini.Models.Payload.Response.Review;
using Galini.Models.Payload.Response.TestHistory;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class TestHistoryMapper : Profile
    {
        public TestHistoryMapper()
        {
            CreateMap<CreateTestHistoryRequest, TestHistory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateTestHistoryRequest, TestHistory>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<TestHistory, CreateTestHistoryResponse>();
        }
    }
}
