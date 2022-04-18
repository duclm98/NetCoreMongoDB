using AutoMapper;
using NetCoreMongoDB.Dtos.Book;
using NetCoreMongoDB.Entities;

namespace NetCoreMongoDB.MappingProfiles;

public class BookMapping : Profile
{
    public BookMapping()
    {
        CreateMap<Book, BookDto>()
            .ReverseMap();
    }
}