using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using WebDataDemo.Model;

namespace WebDataDemo.Option_09_Repo_Spec;

public abstract class AuthorSpecification
{
    public Expression<Func<Author, bool>> Predicate { get; protected set; }
    public Func<IQueryable<Author>, IIncludableQueryable<Author, object>> IncludeExpression { get; protected set; }
}
