using Microsoft.EntityFrameworkCore;
using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;
using SimplyBooksBE.Repositories;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNpgsql<SimplyBooksDbContext>(builder.Configuration["SimplyBooksDbConnectionString"]);
builder.Services.AddScoped<IAuthorsRepository, AuthorsRepository>();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();

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

app.UseRouting();

// ************************************************** CREATE API CALL ************************************************** //

// Create New Book 
app.MapPost("/api/books", async (IBooksRepository repo, Books book) =>
{
    if (book == null)
        return Results.BadRequest("Book cannot be null");

    // Auto create uid and firebaseKey
    book.uid = Guid.NewGuid().ToString();
    book.firebaseKey = Guid.NewGuid().ToString();
    // Check if book already exists by uid
    var existingBook = await repo.GetByIdAsync(book.uid);
    if (existingBook != null)
        return Results.Conflict($"Book with uid {book.uid} already exists");

    // Add the new book
    await repo.AddAsync(book);
    return Results.Created($"/books/{book.uid}", book);
});

// Create New Author
app.MapPost("/api/authors", async (IAuthorsRepository repo, Authors author) =>
{
    if (author == null)
        return Results.BadRequest("Author cannot be null");
    // Auto create uid and firebaseKey

    author.uid = Guid.NewGuid().ToString();
    author.firebaseKey = Guid.NewGuid().ToString();

    // Check if author already exists
    var existingAuthor = await repo.GetByIdAsync(author.uid);
    if (existingAuthor != null)
        return Results.Conflict($"Author with uid {author.uid} already exists");

    // Add the new author
    await repo.AddAsync(author);
    return Results.Created($"/authors/{author.uid}", author);
});

// *************************************************** READ API CALL ************************************************** //

// Get All Books
app.MapGet("/api/books", async (IBooksRepository repo) =>
{
    var books = await repo.GetAllAsync();
    return books.Any() ? Results.Ok(books) : Results.NotFound("No books found");

});

// Get All Authors
app.MapGet("/api/authors", async (IAuthorsRepository repo) =>
{
    var authors = await repo.GetAllAsync();
    return authors.Any() ? Results.Ok(authors) : Results.NotFound("No authors found");
});

// ************************************************** PUT API CALL ************************************************** //

// Update Book
app.MapPut("/api/books/{uid}", async (IBooksRepository repo, string uid, Books updatedBook) =>
{
    if (updatedBook == null)
        return Results.BadRequest("Book cannot be null");
    var book = await repo.GetByIdAsync(uid);
    if (book == null)
        return Results.NotFound($"Book with uid {uid} not found");
    // Update the fields
    book.AuthorId = updatedBook.AuthorId; // Allow updating the AuthorId, if needed
    book.title = updatedBook.title;
    book.description = updatedBook.description;
    book.image = updatedBook.image;
    book.price = updatedBook.price;
    book.sale = updatedBook.sale;
    await repo.UpdateAsync(book);
    return Results.Ok(book);
});

// Update Author
app.MapPut("/api/authors/{uid}", async (IAuthorsRepository repo, string uid, Authors updatedAuthor) =>
{
    if (updatedAuthor == null)
        return Results.BadRequest("Author cannot be null");
    var author = await repo.GetByIdAsync(uid);
    if (author == null)
        return Results.NotFound($"Author with uid {uid} not found");
    // Update the fields
    author.firebaseKey = updatedAuthor.firebaseKey;
    author.first_name = updatedAuthor.first_name;
    author.last_name = updatedAuthor.last_name;
    author.email = updatedAuthor.email;
    author.image = updatedAuthor.image;
    author.favorite = updatedAuthor.favorite;
    await repo.UpdateAsync(author);
    return Results.Ok(author);
});

// ************************************************** DELETE API CALL ************************************************** //

// Delete Book
app.MapDelete("/api/books/{uid}", async (IBooksRepository repo, string uid) =>
{
    var book = await repo.GetByIdAsync(uid);
    if (book == null)
        return Results.NotFound($"Book with uid {uid} not found");

    // Ensure no authors are associated with this book before deleting
    var authorsByBook = await repo.HasAuthorsAsync(uid);
    if (authorsByBook)
        return Results.BadRequest($"Cannot delete book with uid {uid} as there are authors associated with it.");
    await repo.DeleteAsync(uid);
    return Results.Ok($"Book with uid {uid} deleted successfully");
});

// Delete Author
app.MapDelete("/api/authors/{uid}", async (IAuthorsRepository repo, string uid) =>
{
    var author = await repo.GetByIdAsync(uid);
    if (author == null)
        return Results.NotFound($"Author with uid {uid} not found");

    // Ensure no books are associated with this author before deleting
    var booksByAuthor = await repo.HasBooksAsync(uid);
    if (booksByAuthor)
        return Results.BadRequest($"Cannot delete author with uid {uid} as there are books associated with them.");
    await repo.DeleteAsync(uid);
    return Results.Ok($"Author with uid {uid} deleted successfully");

});

app.Run();