using Microsoft.EntityFrameworkCore;
using SimplyBooksBE.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();;


builder.Services.AddNpgsql<SimplyBooksDbContext>(builder.Configuration["SimplyBooksDbConnectionString"]);


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ************************************************** CREATE API CALL ************************************************** //

// Create New Book 
app.MapPost("/api/books", async (SimplyBooksDbContext db, Books book) =>
{
    if (book == null)
        return Results.BadRequest("Book cannot be null");
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.uid}", book);
});

// Create New Author
app.MapPost("/api/authors", async (SimplyBooksDbContext db, Authors author) =>
{
    if (author == null)
        return Results.BadRequest("Author cannot be null");
    db.Authors.Add(author);
    await db.SaveChangesAsync();
    return Results.Created($"/authors/{author.uid}", author);
});

// *************************************************** READ API CALL ************************************************** //

// Get All Books
app.MapGet("/api/books", async (SimplyBooksDbContext db) =>
{
    var books = await db.Books
        .Include(b => b.Author) // Include the Author navigation property
        .ToListAsync();
    return Results.Ok(books);
});

// Get All Authors
app.MapGet("/api/authors", async (SimplyBooksDbContext db) =>
{
    var authors = await db.Authors.ToListAsync();
    if (authors == null || authors.Count == 0)
        return Results.NotFound("No authors found");
    return Results.Ok(authors);
});

// ************************************************** PUT API CALL ************************************************** //

// Update Book
app.MapPut("/api/books/{uid}", async (SimplyBooksDbContext db, string uid, Books updatedBook) =>
{
    if (updatedBook == null)
        return Results.BadRequest("Book cannot be null");
    var book = await db.Books.FindAsync(uid);
    if (book == null)
        return Results.NotFound($"Book with uid {uid} not found");
    // Update the fields
    book.AuthorId = updatedBook.AuthorId; // Allow updating the AuthorId, if needed
    book.title = updatedBook.title;
    book.description = updatedBook.description;
    book.image = updatedBook.image;
    book.price = updatedBook.price;
    book.sale = updatedBook.sale;
    await db.SaveChangesAsync();
    return Results.Ok(book);
});

// Update Author
app.MapPut("/api/authors/{uid}", async (SimplyBooksDbContext db, string uid, Authors updatedAuthor) =>
{
    if (updatedAuthor == null)
        return Results.BadRequest("Author cannot be null");
    var author = await db.Authors.FindAsync(uid);
    if (author == null)
        return Results.NotFound($"Author with uid {uid} not found");
    // Update the fields
    author.firebaseKey = updatedAuthor.firebaseKey;
    author.first_name = updatedAuthor.first_name;
    author.last_name = updatedAuthor.last_name;
    author.email = updatedAuthor.email;
    author.image = updatedAuthor.image;
    await db.SaveChangesAsync();
    return Results.Ok(author);
});

// ************************************************** DELETE API CALL ************************************************** //

// Delete Book
app.MapDelete("/api/books/{uid}", async (SimplyBooksDbContext db, string uid) =>
{
    var book = await db.Books.FindAsync(uid);
    if (book == null)
        return Results.NotFound($"Book with uid {uid} not found");
    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.Ok($"Book with uid {uid} deleted successfully");
});

// Delete Author
app.MapDelete("/api/authors/{uid}", async (SimplyBooksDbContext db, string uid) =>
{
    var author = await db.Authors.FindAsync(uid);
    if (author == null)
        return Results.NotFound($"Author with uid {uid} not found");

    // Ensure no books are associated with this author before deleting
    var booksByAuthor = await db.Books.CountAsync(b => b.AuthorId == uid);
    if (booksByAuthor > 0)
        return Results.BadRequest($"Cannot delete author with uid {uid} as there are books associated with them.");
    db.Authors.Remove(author);
    await db.SaveChangesAsync();
    return Results.Ok($"Author with uid {uid} deleted successfully");
});

app.Run();