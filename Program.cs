using Microsoft.EntityFrameworkCore;
using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;
using SimplyBooksBE.Repositories;
using SimplyBooksBE.Endpoints;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using SimplyBooksBE.Services; // For JsonOptions

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNpgsql<SimplyBooksDbContext>(builder.Configuration["SimplyBooksDbConnectionString"]);
builder.Services.AddScoped<IAuthorsRepository, IAuthorService>();
builder.Services.AddScoped<IBooksRepository, IBookService>();


// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAuthorsEndpoints();
app.MapBooksEndpoints();

app.Run();
