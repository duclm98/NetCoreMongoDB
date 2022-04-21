using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Dtos.Category;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Helpers.Result;
using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Services;

public interface ICategoryService
{
    Task<IActionResult> GetAll();
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
        var categories = await _context.Query<Category>(null)
            .ToListAsync();
        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
        return new CustomResult("Thành công", new Result
        {
            Data = categoryDtos
        });
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