using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.WorkShift;
using Galini.Models.Payload.Response.WorkShift;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class WorkShipMapper : Profile
    {
        public WorkShipMapper()
        {
            CreateMap<CreateWorkShiftRequest, WorkShift>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateWorkShiftRequest, Premium>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Premium, CreateWorkShiftResponse>();
        }
    }
}
