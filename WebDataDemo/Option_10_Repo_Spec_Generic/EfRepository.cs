using Ardalis.Specification.EntityFrameworkCore;
using WebDataDemo.Data;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
