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
                .ForMember(x=>x.Category, opt=>opt.MapFrom(x=>x.Category.Title));


        }
    }
}
