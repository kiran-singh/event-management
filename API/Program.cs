using System.Reflection;
using AutoMapper;
using EventManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddRepositories(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Services.GetService<IMapper>()!
    .ConfigurationProvider
    .AssertConfigurationIsValid();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();