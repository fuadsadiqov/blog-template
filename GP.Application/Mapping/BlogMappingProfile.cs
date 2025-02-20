using AutoMapper;
using GP.Application.BlogQueries;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class BlogDetailMappingProfile : Profile
    {
        public BlogDetailMappingProfile()
        {
            CreateMap<Blog, BlogResponse>()
                .ForPath(x=>x.Category.Id, opt => opt.MapFrom(x=>x.Category.Id))
                .ForPath(x=>x.Category.Title, opt=>opt.MapFrom(x=>x.Category.Title));
        }
    }
}
