using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Booking;
using Galini.Models.Payload.Response.Booking;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class BookingMapper : Profile
    {
        public BookingMapper()
        {
            CreateMap<CreateBookingRequest, Booking>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != BookingEnum.Upcoming));

            CreateMap<UpdateBookingRequest, Booking>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != BookingEnum.None));

            CreateMap<Booking, CreateBookingResponse>();
        }
    }
}
