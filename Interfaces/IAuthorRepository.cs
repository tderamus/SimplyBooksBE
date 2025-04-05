using SimplyBooksBE.Models;

namespace SimplyBooksBE.Interfaces
{
    public interface IAuthorsRepository
    {
        Task<IEnumerable<Authors>> GetAllAsync();
        Task<Authors?> GetByIdAsync(string uid);
        Task AddAsync(Authors author);
        Task UpdateAsync(Authors author);
        Task DeleteAsync(string uid);
        Task<bool> HasBooksAsync(string authorId);
        Task<Authors> GetAuthorBooksAsync(string id);
    }
}