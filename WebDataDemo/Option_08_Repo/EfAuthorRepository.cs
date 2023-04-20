using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebDataDemo.Data;
using WebDataDemo.Model;

namespace WebDataDemo.Option_08_Repo;

public class EfAuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _dbContext;

    public EfAuthorRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Author> GetByIdAsync(int id)
    {
        return _dbContext.Authors
            .FirstOrDefaultAsync(author => author.Id == id);
    }

    public async Task<IEnumerable<Author>> List(Expression<Func<Author, bool>> predicate)
    {
        return await _dbContext.Authors
            .Where(predicate)
            .ToListAsync();
    }

    public IQueryable<Author> List()
    {
        return _dbContext.Authors.AsQueryable();
    }

    public async Task CreateAsync(Author newAuthor)
    {
        await _dbContext.Authors.AddAsync(newAuthor);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(Author author)
    {
        return _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Author author)
    {
        _dbContext.Remove(author);
        return _dbContext.SaveChangesAsync();
    }
}
