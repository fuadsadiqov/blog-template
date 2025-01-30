using AutoMapper;
using GP.Application.CategoryQueries;
using GP.Domain.Entities.Common;

namespace GP.Application.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryResponse>();
        }
    }
}
