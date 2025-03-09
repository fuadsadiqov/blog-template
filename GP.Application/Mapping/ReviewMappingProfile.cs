using AutoMapper;
using GP.Application.BlogQueries;
using GP.Application.Queries.BlogQueries;
using GP.Domain.Entities.Common;
using GP.Domain.Entities.Identity;

namespace GP.Application.Mapping
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<Review, BlogReview>()
                .ForPath(b => b.User.Id, opt => opt.MapFrom(t => t.User.Id))
                .ForPath(b => b.User.FullName, opt => opt.MapFrom(t => t.User.FullNameAz))
                .ForPath(b => b.User.IsAuthReview, opt => opt.Ignore());
        }
    }
}
