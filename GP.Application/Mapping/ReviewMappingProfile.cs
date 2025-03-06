using AutoMapper;
using GP.Application.BlogQueries;
using GP.Application.Queries.BlogQueries;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<BlogReview, Review>()
                .ForMember(b => b.User, opt => opt.MapFrom(t => t.User));
        }
    }
}
