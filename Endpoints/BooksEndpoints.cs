using Microsoft.AspNetCore.Builder;
using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;

namespace SimplyBooksBE.Endpoints
{
    public static class BooksEndpoints
    {
        public static void MapBooksEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Create New Book
            endpoints.MapPost("/api/books", async (IBooksRepository repo, Books book) =>
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

            // Get All Books
            endpoints.MapGet("/api/books", async (IBooksRepository repo) =>
            {
                var books = await repo.GetAllAsync();
                return books.Any() ? Results.Ok(books) : Results.NotFound("No books found");
            });

            // Update Book
            endpoints.MapPut("/api/books/{uid}", async (IBooksRepository repo, string uid, Books updatedBook) =>
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

            // Delete Book
            endpoints.MapDelete("/api/books/{uid}", async (IBooksRepository repo, string uid) =>
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

            // Remove asscoitated author from book before deleting it
            endpoints.MapDelete("/api/books/{uid}/remove-author", async (IBooksRepository repo, string uid) =>
            {
                var book = await repo.GetByIdAsync(uid);
                if (book == null)
                    return Results.NotFound($"Book with uid {uid} not found");
                // Remove the association with the author
                book.AuthorId = null; // Or set to a default value
                await repo.UpdateAsync(book);
                return Results.Ok($"Author removed from book with uid {uid}");
            });

        }
    }
}
