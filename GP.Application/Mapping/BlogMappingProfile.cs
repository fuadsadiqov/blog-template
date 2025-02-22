using AutoMapper;
using GP.Application.BlogQueries;
using GP.Application.Queries.BlogQueries;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<Blog, BlogDetailResponse>()
                .ForPath(x=>x.Category.Id, opt => opt.MapFrom(x=>x.Category.Id))
                .ForPath(x=>x.Category.Title, opt=>opt.MapFrom(x=>x.Category.Title))
                .ForMember(b => b.Tags, opt => opt.MapFrom(t => t.Tags))
                .ForMember(b => b.Reviews, opt => opt.MapFrom(t => t.Reviews));

            CreateMap<Blog, BlogResponse>()
                .ForPath(x=>x.Category.Id, opt => opt.MapFrom(x=>x.Category.Id))
                .ForPath(x=>x.Category.Title, opt=>opt.MapFrom(x=>x.Category.Title));
        }
    }
}
