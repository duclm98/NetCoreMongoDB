using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Dtos.Book;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.Helpers.Result;
using NetCoreMongoDB.Models;
using System.Text.RegularExpressions;

namespace NetCoreMongoDB.Services;

public interface IBookService
{
    Task<IActionResult> GetAll(BookQuery query);
    Task<IActionResult> GetSingle(string bookId);
    Task<IActionResult> Create(BookCreateDto bookCreateDto);
    Task<IActionResult> Update(string bookId, BookUpdateDto bookUpdateDto);
    Task<IActionResult> Delete(string bookId);
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

    public async Task<IActionResult> GetAll(BookQuery query)
    {
        var filterDefinitionBuilder = Builders<Book>.Filter;
        var filterDefinition = filterDefinitionBuilder.Empty;
        if (!string.IsNullOrEmpty(query.BookName))
            filterDefinition &= filterDefinitionBuilder.Regex(x => x.BookName, new BsonRegularExpression(new Regex(query.BookName, RegexOptions.None)));
        if (query.Price != 0)
            filterDefinition &= filterDefinitionBuilder.Eq(x => x.Price, query.Price);

        var bookQuery = _context.Query(filterDefinition)
            .SortByDescending(x => x.CreatedAt);

        var totalRecord = await bookQuery.CountDocumentsAsync();
        var books = await bookQuery.Skip((query.Page - 1) * query.PageCount).Limit(query.PageCount).ToListAsync();
        var booksDtos = _mapper.Map<List<BookDto>>(books);

        return new CustomResult("Thành công", new Result
        {
            TotalRecord = totalRecord,
            Data = booksDtos,
            PageCount = query.PageCount,
            Page = query.Page,
        });
    }

    public async Task<IActionResult> GetSingle(string bookId)
    {
        //var bookCollection = _context.Collection<Book>();
        //var result = await bookCollection.Aggregate()
        //    .Lookup("Category", "CategoryId", "_id", "Category")
        //    .FirstOrDefaultAsync();

        var book = await _context.FindAsync<Book>(bookId);
        if (book is null)
            throw new CustomException("Sách không tồn tại", 404);
        var booksDto = _mapper.Map<BookDto>(book);
        return new CustomResult("Thành công", booksDto);
    }

    public async Task<IActionResult> Create(BookCreateDto bookCreateDto)
    {
        var category = await _context.FindAsync<Category>(bookCreateDto.CategoryId);
        if (category == null)
            throw new CustomException("Danh mục sách không tồn tại", 404);

        var book = new Book
        {
            BookName = bookCreateDto.BookName,
            Price = bookCreateDto.Price,
            CategoryId = category.Id,
            Author = bookCreateDto.Author
        };
        await _context.InsertAsync(book);
        var booksDto = _mapper.Map<BookDto>(book);

        return new CustomResult("Tạo sách thành công", booksDto, 201);
    }

    public async Task<IActionResult> Update(string bookId, BookUpdateDto bookUpdateDto)
    {
        var book = await _context.FindAsync<Book>(bookId);
        if (book == null)
            throw new CustomException("Sách không tồn tại!", 404);

        book.BookName = bookUpdateDto.BookName ?? book.BookName;
        book.Price = bookUpdateDto.Price != 0 ? bookUpdateDto.Price : book.Price;
        book.Author = bookUpdateDto.Author ?? book.Author;
        await _context.UpdateAsync(book);
        var booksDto = _mapper.Map<BookDto>(book);

        return new CustomResult("Chỉnh sửa sách thành công", booksDto);
    }

    public async Task<IActionResult> Delete(string bookId)
    {
        var book = await _context.FindAsync<Book>(bookId);
        if (book == null)
            throw new CustomException("Sách không tồn tại!", 404);

        await _context.DeleteAsync(book);
        var booksDto = _mapper.Map<BookDto>(book);
        return new CustomResult("Xóa sách thành công", booksDto);
    }
}