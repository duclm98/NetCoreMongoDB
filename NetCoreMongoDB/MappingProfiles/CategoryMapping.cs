using AutoMapper;
using NetCoreMongoDB.Dtos;
using NetCoreMongoDB.Dtos.Category;
using NetCoreMongoDB.Entities;

namespace NetCoreMongoDB.MappingProfiles;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>()
        .IncludeBase<Base, BaseDto>()
        .ForMember(x => x.CategoryId, y => y.MapFrom(z => z.Id))
        .ReverseMap();
    }
}