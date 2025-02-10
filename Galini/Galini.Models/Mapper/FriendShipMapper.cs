using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.FriendShip;
using Galini.Models.Payload.Request.UserCall;
using Galini.Models.Payload.Response.FriendShip;
using Galini.Models.Payload.Response.UserCall;
using Galini.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Mapper
{
    public class FriendShipMapper : Profile
    {
        public FriendShipMapper()
        {
            CreateMap<UpdateFriendShipRequest, FriendShip>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != FriendShipEnum.None));

            CreateMap<FriendShip, CreateFriendShipResponse>();
        }
    }
}
