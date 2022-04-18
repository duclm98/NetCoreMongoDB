using Microsoft.AspNetCore.Mvc;
using NetCoreMongoDB.Dtos.Book;
using NetCoreMongoDB.Services;

namespace NetCoreMongoDB.Controllers;

[ApiController]
[Route("v1/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _booksService;

    public BooksController(IBookService booksService) =>
        _booksService = booksService;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await _booksService.GetAll();

    [HttpGet("{bookId:length(24)}")]
    public async Task<IActionResult> GetSingle(string bookId) =>
        await _booksService.GetSingle(bookId);

    [HttpPost]
    public async Task<IActionResult> Create(BookCreateDto bookCreateDto) =>
        await _booksService.Create(bookCreateDto);

    //[HttpPut("{id:length(24)}")]
    //public async Task<IActionResult> Update(string id, Book updatedBook)
    //{
    //    var book = await _booksService.GetAsync(id);

    //    if (book is null)
    //    {
    //        return NotFound();
    //    }

    //    updatedBook.Id = book.Id;

    //    await _booksService.UpdateAsync(id, updatedBook);

    //    return NoContent();
    //}

    //[HttpDelete("{id:length(24)}")]
    //public async Task<IActionResult> Delete(string id)
    //{
    //    var book = await _booksService.GetAsync(id);

    //    if (book is null)
    //    {
    //        return NotFound();
    //    }

    //    await _booksService.RemoveAsync(id);

    //    return NoContent();
    //}
}