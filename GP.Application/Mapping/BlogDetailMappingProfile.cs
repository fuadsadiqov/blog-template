using AutoMapper;
using GP.Core.Models;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<Blog, BlogDetailModel>()
                .ForMember(b => b.Category, opt => opt.MapFrom(b => b.Category.Title))
                .ForMember(b => b.Tags, opt => opt.MapFrom(t => t.Tags));

        }
    }
}
