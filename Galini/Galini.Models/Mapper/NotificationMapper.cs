using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Payload.Request.Notification;
using Galini.Models.Payload.Response.Notification;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<CreateNotificationRequest, Notification>()
                .ForMember(dest => dest.Id, otp => otp.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsRead, otp => otp.MapFrom(src => false))
                .ForMember(dest => dest.IsActive, otp => otp.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Notification, CreateNotificationResponse>();
        }
    }
}
