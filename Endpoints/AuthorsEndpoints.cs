using Microsoft.AspNetCore.Builder;
using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;

namespace SimplyBooksBE.Endpoints
{
    public static class AuthorsEndpoints
    {
        public static void MapAuthorsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Create New Author
            endpoints.MapPost("/api/authors", async (IAuthorsRepository repo, Authors author) =>
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

            // Get All Authors
            endpoints.MapGet("/api/authors", async (IAuthorsRepository repo) =>
            {
                var authors = await repo.GetAllAsync();
                return authors.Any() ? Results.Ok(authors) : Results.NotFound("No authors found");
            });

            // Update Author
            endpoints.MapPut("/api/authors/{uid}", async (IAuthorsRepository repo, string uid, Authors updatedAuthor) =>
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

            // Delete Author
            endpoints.MapDelete("/api/authors/{uid}", async (IAuthorsRepository repo, string uid) =>
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

            // Remove associated books from author before deleting it
            endpoints.MapDelete("/api/authors/{uid}/remove-books", async (IAuthorsRepository repo, string uid) =>
            {
                var author = await repo.GetByIdAsync(uid);
                if (author == null)
                    return Results.NotFound($"Author with uid {uid} not found");
                // Remove all associated books
                author.Books?.Clear(); // Clear the navigation property
                await repo.UpdateAsync(author); // Save changes to the author
                return Results.Ok($"All books associated with author {uid} have been removed.");
            });
        }
    }
}
