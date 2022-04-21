using Microsoft.AspNetCore.Mvc;
using NetCoreMongoDB.Dtos.Category;
using NetCoreMongoDB.Services;

namespace NetCoreMongoDB.Controllers;

[Route("v1/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService) =>
        _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        await _categoryService.GetAll();

    //[HttpGet("{bookId:length(24)}")]
    //public async Task<IActionResult> GetSingle(string bookId) =>
    //    await _bookService.GetSingle(bookId);

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto) =>
        await _categoryService.Create(categoryCreateDto);
}
