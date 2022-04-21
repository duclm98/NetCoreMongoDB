using AutoMapper;
using NetCoreMongoDB.Dtos;
using NetCoreMongoDB.Dtos.Book;
using NetCoreMongoDB.Entities;

namespace NetCoreMongoDB.MappingProfiles;

public class BookMapping : Profile
{
    public BookMapping()
    {
        CreateMap<Book, BookDto>()
            .IncludeBase<Base, BaseDto>()
            .ForMember(x => x.BookId, y => y.MapFrom(z => z.Id))
            .ReverseMap();
    }
}