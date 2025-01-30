using AutoMapper;
using GP.Application.TagQueries;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class TagMappingProfile : Profile
    {
        public TagMappingProfile()
        {
            CreateMap<Tag, TagResponse>();
        }
    }
}
