namespace SimplyBooksBE.Interfaces;
using SimplyBooksBE.Models;

public class IAuthorService : IAuthorsRepository
{
    private readonly SimplyBooksDbContext _context;
    public IAuthorService(SimplyBooksDbContext context)
    {
        _context = context;
    }
    public Task<Authors> AddAsync(Authors author)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(string uid)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<Authors>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Authors> GetAuthorBooksAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Authors> GetByIdAsync(string uid)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasBooksAsync(string authorId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Authors author)
    {
        throw new NotImplementedException();
    }

    Task IAuthorsRepository.AddAsync(Authors author)
    {
        throw new NotImplementedException();
    }
}

