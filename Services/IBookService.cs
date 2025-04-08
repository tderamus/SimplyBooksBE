using SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;

namespace SimplyBooksBE.Services
{
    public class IBookService : IBooksRepository
    {
        private readonly SimplyBooksDbContext _context;
        public IBookService(SimplyBooksDbContext context)
        {
            _context = context;
        }
        public Task<Books> AddAsync(Books book)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(string uid)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Books>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Books> GetByIdAsync(string uid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAuthorsAsync(string bookId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Books book)
        {
            throw new NotImplementedException();
        }

        Task IBooksRepository.AddAsync(Books book)
        {
            throw new NotImplementedException();
        }
    }
}
