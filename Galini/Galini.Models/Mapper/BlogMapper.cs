using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Galini.Models.Entity;
using Galini.Models.Enum;
using Galini.Models.Payload.Request.Blog;
using Galini.Models.Payload.Response.Blog;
using Galini.Utils;

namespace Galini.Models.Mapper
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<CreateBlogRequest, Blog>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Views, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => BlogTypeEnum.Customer.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BlogStatusEnum.PENDING.GetDescriptionFromEnum()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Blog, CreateBlogResponse>();
            CreateMap<Blog, GetBlogResponse>();

        }
    }
}
