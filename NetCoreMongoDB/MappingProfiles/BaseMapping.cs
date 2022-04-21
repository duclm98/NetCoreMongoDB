using AutoMapper;
using NetCoreMongoDB.Dtos;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Extensions;

namespace NetCoreMongoDB.MappingProfiles;

public class BaseMapping : Profile
{
    public BaseMapping()
    {
        CreateMap<Base, BaseDto>()
            .ForMember(x => x.CreatorId, y => y.MapFrom(z => z.Creator.CreatorId))
            .ForMember(x => x.CreatorName, y => y.MapFrom(z => z.Creator.CreatorName))
            .ForMember(x => x.CreatorRole, y => y.MapFrom(z => z.Creator.CreatorRole.GetValue()))
            .ReverseMap();
    }
}