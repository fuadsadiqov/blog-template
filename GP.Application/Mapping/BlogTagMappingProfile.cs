using AutoMapper;
using GP.Application.BlogTagQueries;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class BlogTagMappingProfile : Profile
    {
        public BlogTagMappingProfile()
        {
            CreateMap<BlogTag, BlogTagResponse>()
                .ForMember(pT => pT.Name, opt => opt.MapFrom(x => x.Tag.Name))
                .ForMember(pT => pT.Id, opt => opt.MapFrom(x => x.Tag.Id));
        }
    }
}
