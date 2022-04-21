using MongoDB.Driver;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.SubServices;

namespace NetCoreMongoDB.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IUserSubService userSubService, DatabaseContext context)
    {
        string[] ignorePath =
        {
                "/v1/auth/login",
                "/v1/auth/register",
            };

        if (!ignorePath.Contains(httpContext.Request.Path.Value))
        {
            var accessToken = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (accessToken == null)
                throw new CustomException("Thiếu access token!", 401);

            var userId = userSubService.ValidateJsonWebToken(accessToken);

            if (userId == null)
                throw new CustomException("Access token không hợp lệ hoặc đã hết hạn!", 401);

            var userQuery = context.Query(Builders<User>.Filter.Where(x => x.Id == userId));

            if (!await userQuery.AnyAsync())
                throw new CustomException("Không xác thực bảo mật!", 401);

            httpContext.Items["userId"] = userId;
        }

        await _next(httpContext);
    }
}