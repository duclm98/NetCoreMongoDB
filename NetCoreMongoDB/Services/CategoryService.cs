using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Dtos.Category;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.Helpers.Result;
using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Services;

public interface ICategoryService
{
    Task<IActionResult> GetAll();
    Task<IActionResult> GetSingle(string categoryId);
    Task<IActionResult> Create(CategoryCreateDto categoryCreateDto);
}

public class CategoryService : ICategoryService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public CategoryService(IComponentContext componentContext)
    {
        _context = componentContext.Resolve<DatabaseContext>();
        _mapper = componentContext.Resolve<IMapper>();
    }

    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Query<Category>()
            .ToListAsync();
        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
        return new CustomResult("Thành công", new Result
        {
            Data = categoryDtos
        });
    }

    public async Task<IActionResult> GetSingle(string categoryId)
    {
        //var categoryCollection = _context.Collection<Category>();
        //var bookCollection = _context.Collection<Book>();
        //var result = await categoryCollection.Aggregate()
        //    .Lookup<Category, Book, CategoryLookedUp>(
        //        bookCollection,
        //        x => x.Id,
        //        x => x.CategoryId,
        //        x => x.Books
        //    )
        //    .ToListAsync();

        var category = await _context.FindAsync<Category>(categoryId);
        if (category is null)
            throw new CustomException("Danh mục sách không tồn tại", 404);
        var categoryDto = _mapper.Map<CategoryDto>(category);
        return new CustomResult("Thành công", categoryDto);
    }

    public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
    {
        var category = new Category
        {
            CategoryName = categoryCreateDto.CategoryName
        };
        await _context.InsertAsync(category);
        var categoryDto = _mapper.Map<CategoryDto>(category);

        return new CustomResult("Tạo sách thành công", categoryDto, 201);
    }
}