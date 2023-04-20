using Microsoft.EntityFrameworkCore;
using WebDataDemo.Data;
using WebDataDemo.Model;

namespace WebDataDemo.Option_09_Repo_Spec;

public class EfAuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _dbContext;

    public EfAuthorRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Author> GetBySpecAsync(AuthorSpecification spec)
    {
        var query = _dbContext.Authors.AsQueryable();

        if (spec.IncludeExpression != null)
        {
            query = spec.IncludeExpression(query);
        }
        return query.FirstOrDefaultAsync(spec.Predicate);
    }

    public async Task<IEnumerable<Author>> List(AuthorSpecification spec)
    {
        var query = _dbContext.Set<Author>().AsQueryable();

        if (spec != null && spec.Predicate != null)
        {
            query = query.Where(spec.Predicate);
        }
        return await query.ToListAsync();
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
