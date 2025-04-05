using SimplyBooksBE.Models;

namespace SimplyBooksBE.Interfaces
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Books>> GetAllAsync();
        Task<Books?> GetByIdAsync(string uid);
        Task AddAsync(Books book);
        Task UpdateAsync(Books book);
        Task DeleteAsync(string uid);
        Task<bool> HasAuthorsAsync(string bookId);
    }
}
