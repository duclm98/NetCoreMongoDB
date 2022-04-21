using Microsoft.AspNetCore.Mvc;
using NetCoreMongoDB.Dtos.Book;
using NetCoreMongoDB.Services;

namespace NetCoreMongoDB.Controllers;

[ApiController]
[Route("v1/books")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService) =>
        _bookService = bookService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BookQuery query) =>
        await _bookService.GetAll(query);

    [HttpGet("{bookId:length(24)}")]
    public async Task<IActionResult> GetSingle(string bookId) =>
        await _bookService.GetSingle(bookId);

    [HttpPost]
    public async Task<IActionResult> Create(BookCreateDto bookCreateDto) =>
        await _bookService.Create(bookCreateDto);

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, BookUpdateDto bookUpdateDto) =>
        await _bookService.Update(id, bookUpdateDto);

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id) =>
        await _bookService.Delete(id);
}