using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Premium;
using Galini.Models.Payload.Request.UserInfo;
using Galini.Models.Payload.Response.Premium;
using Galini.Models.Payload.Response.UserInfo;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class PremiumMapper : Profile
    {
        public PremiumMapper()
        {
            CreateMap<CreatePremiumRequest, Premium>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdatePremiumRequest, Premium>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Premium, CreatePremiumResponse>();
        }
    }
}
