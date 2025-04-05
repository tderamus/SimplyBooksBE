using Microsoft.EntityFrameworkCore;
using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;

namespace SimplyBooksBE.Repositories;
public class AuthorsRepository : IAuthorsRepository
{
    private readonly SimplyBooksDbContext _context;
    public AuthorsRepository(SimplyBooksDbContext context) => _context = context;

    public async Task<IEnumerable<Authors>> GetAllAsync()
    {
        return await _context.Authors
            .Include(a => a.Books) // Include the Books navigation property
            .ToListAsync();
    }

    public async Task<Authors?> GetByIdAsync(string uid)
    {
        return await _context.Authors
            .Include(a => a.Books) // Include the Books navigation property
            .FirstOrDefaultAsync(a => a.uid == uid);
    }

    public async Task AddAsync(Authors author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Authors author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string uid)
    {
        var author = await _context.Authors.FindAsync(uid);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasBooksAsync(string authorId)
    {
        return await _context.Books.AnyAsync(b => b.AuthorId == authorId);
    }

    public async Task<Authors> GetAuthorBooksAsync(string id)
    {
        return await _context.Authors
            .Include(a => a.Books) // Include the Books navigation property
            .FirstOrDefaultAsync(a => a.uid == id);
    }

    public async Task<List<Authors>> GetAllAuthorsAsync()
    {
        return await _context.Authors
            .Include(a => a.Books) // Include the Books navigation property
            .ToListAsync();
    }
}

