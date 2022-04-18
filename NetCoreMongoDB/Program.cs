using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using NetCoreMongoDB.Context;
using NetCoreMongoDB.Helpers.Exceptions;
using NetCoreMongoDB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    // AutoMapper
    builder.RegisterAutoMapper(typeof(Program).Assembly);

    // Service
    builder.RegisterAssemblyTypes(typeof(BooksService).Assembly)
        .Where(x => x.Name.EndsWith("Service"))
        .AsImplementedInterfaces().InstancePerLifetimeScope();
});

builder.Services.AddSingleton<DatabaseContext>();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

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
app.MapControllers();
app.Run();