using Microsoft.EntityFrameworkCore;
using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;

namespace SimplyBooksBE.Services
{
    public class BookService : IBooksRepository
    {
        private readonly SimplyBooksDbContext _context;
        public BookService(SimplyBooksDbContext context) => _context = context;

        public async Task<IEnumerable<Books>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author) // Include the Author navigation property
                .ToListAsync();
        }

        public async Task<Books?> GetByIdAsync(string uid)
        {
            return await _context.Books
                .Include(b => b.Author) // Include the Author navigation property
                .FirstOrDefaultAsync(b => b.uid == uid);
        }

        public async Task AddAsync(Books book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Books book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string uid)
        {
            var book = await GetByIdAsync(uid);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasAuthorsAsync(string bookId)
        {
            return await _context.Books
                .AnyAsync(b => b.uid == bookId && b.AuthorId != null);
        }
    }
}
