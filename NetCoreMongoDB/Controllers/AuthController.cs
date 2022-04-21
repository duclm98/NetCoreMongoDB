using Microsoft.AspNetCore.Mvc;
using NetCoreMongoDB.Dtos.Authentication;
using NetCoreMongoDB.Services;

namespace NetCoreMongoDB.Controllers;

[ApiController]
[Route("v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService) =>
        _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCreateDto registerCreateDto) =>
        await _userService.Register(registerCreateDto);

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCreateDto loginCreateDto) =>
        await _userService.Login(loginCreateDto);
}