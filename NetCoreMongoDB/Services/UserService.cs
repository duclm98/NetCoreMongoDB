using Autofac;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Dtos.Authentication;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.Helpers.Result;
using NetCoreMongoDB.SubServices;

namespace NetCoreMongoDB.Services;

public interface IUserService
{
    Task<IActionResult> Register(RegisterCreateDto registerCreateDto);
    Task<IActionResult> Login(LoginCreateDto loginCreateDto);
}

public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly IUserSubService _userSubService;

    public UserService(IComponentContext componentContext)
    {
        _context = componentContext.Resolve<DatabaseContext>();
        _userSubService = componentContext.Resolve<IUserSubService>();
    }

    public async Task<IActionResult> Register(RegisterCreateDto registerCreateDto)
    {
        if (registerCreateDto.Username == null || registerCreateDto.Password == null)
            throw new CustomException("Tên đăng nhập hoặc mật khẩu không được trống", 400);

        var user = await _context.Query(Builders<User>.Filter.Where(x => x.Username == registerCreateDto.Username))
            .FirstOrDefaultAsync();
        if (user != null)
            throw new CustomException("Tên đăng nhập đã tồn tại", 409);

        var newUser = new User
        {
            Username = registerCreateDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(registerCreateDto.Password, 5),
            Fullname = registerCreateDto.Fullname,
            Role = UserRole.Customer
        };
        await _context.InsertAsync(newUser);

        return new CustomResult("Đăng ký tài khoản thành công", new
        {
            User = new
            {
                UserId = newUser.Id,
                newUser.Username
            }
        });
    }

    public async Task<IActionResult> Login(LoginCreateDto loginCreateDto)
    {
        var user = await _context.Query(Builders<User>.Filter.Where(x => x.Username == loginCreateDto.Username))
            .FirstOrDefaultAsync();
        if (user == null)
            throw new CustomException("Tên đăng nhập hoặc mật khẩu không chính xác", 401);

        if (!BCrypt.Net.BCrypt.Verify(loginCreateDto.Password, user.Password))
            throw new CustomException("Tên đăng nhập hoặc mật khẩu không chính xác", 401);

        var accessToken = _userSubService.GenerateJsonWebToken(user.Id);

        return new CustomResult("Đăng nhập thành công", new
        {
            User = new
            {
                UserId = user.Id,
                user.Username
            },
            AccessToken = accessToken
        });
    }
}