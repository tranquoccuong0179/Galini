using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Question;
using Galini.Models.Payload.Response.Question;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class QuestionMapper : Profile
    {
        public QuestionMapper()
        {
            CreateMap<CreateQuestionRequest, Question>()
                .ForMember(dest => dest.Id, otp => otp.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, otp => otp.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Question, CreateQuestionResponse>();
            CreateMap<Question, GetQuestionResponse>();
        }
    }
}
