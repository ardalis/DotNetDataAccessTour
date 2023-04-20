using Microsoft.EntityFrameworkCore;
using WebDataDemo.Data;
using WebDataDemo.Model;

namespace WebDataDemo.Option_06_Repo;

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

    public Task<Author> GetByIdAsyncWithCourses(int id)
    {
        return _dbContext.Authors
            .Include(author => author.Courses)
            .ThenInclude(ca => ca.Course)
            .FirstOrDefaultAsync(author => author.Id == id);
    }

    public async Task<IEnumerable<Author>> ListAsync()
    {
        return await _dbContext.Authors.ToListAsync();
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
