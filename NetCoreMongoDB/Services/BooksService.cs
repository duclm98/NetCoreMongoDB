using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Dtos.Book;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.Helpers.Result;

namespace NetCoreMongoDB.Services;

public interface IBookService
{
    Task<IActionResult> GetAll();
    Task<IActionResult> GetSingle(string bookId);
    Task<IActionResult> Create(BookCreateDto bookCreateDto);
}

public class BooksService : IBookService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public BooksService(IComponentContext componentContext)
    {
        _context = componentContext.Resolve<DatabaseContext>();
        _mapper = componentContext.Resolve<IMapper>();
    }

    public async Task<IActionResult> GetAll()
    {
        var books = await _context.Books.AsQueryable()
            .ToListAsync();
        var booksDtos = _mapper.Map<List<BookDto>>(books);
        return new CustomResult("Thành công", booksDtos);
    }

    public async Task<IActionResult> GetSingle(string bookId)
    {
        var book = await _context.Books.Find(x => x.BookId == bookId)
            .FirstOrDefaultAsync();
        if (book is null)
            throw new CustomException("Không tìm thấy sách", 404);
        var booksDto = _mapper.Map<BookDto>(book);
        return new CustomResult("Thành công", booksDto);
    }

    public async Task<IActionResult> Create(BookCreateDto bookCreateDto)
    {
        var book = new Book
        {
            BookName = bookCreateDto.BookName,
            Price = bookCreateDto.Price,
            Category = bookCreateDto.Category,
            Author = bookCreateDto.Author
        };
        await _context.Books.InsertOneAsync(book);
        var booksDto = _mapper.Map<BookDto>(book);
        return new CustomResult("Tạo sách thành công", booksDto, 201);
    }
}