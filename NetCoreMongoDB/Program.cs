using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Microsoft.AspNetCore.HttpLogging;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.Middlewares;
using NetCoreMongoDB.Services;
using NetCoreMongoDB.SubServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        // AutoMapper
        builder.RegisterAutoMapper(typeof(Program).Assembly);

        // Service
        builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
            .Where(x => x.Name.EndsWith("Service"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();

        // Sub Service
        builder.RegisterAssemblyTypes(typeof(UserSubService).Assembly)
        .Where(x => x.Name.EndsWith("SubService"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    });

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
    //config.WriteTo.MongoDB(builder.Configuration["LoggingDatabse:DatabaseURL"], builder.Configuration["LoggingDatabse:CollectionName"]);
    config.WriteTo.File("./Logs/logfile-.txt", rollingInterval: RollingInterval.Day);
});

// Thiết lập vị trí lưu audit log
Audit.Core.Configuration.DataProvider = new Audit.MongoDB.Providers.MongoDataProvider()
{
    ConnectionString = builder.Configuration["BookStoreDatabase:ConnectionString"],
    Database = builder.Configuration["BookStoreDatabase:DatabaseName"],
    Collection = "AuditLog"
};

builder.Services.AddSingleton<DatabaseContext>();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCors(config =>
{
    config.AllowAnyOrigin();
    config.AllowAnyHeader();
    config.AllowAnyMethod();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CustomExceptionMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.MapControllers();

app.Run();