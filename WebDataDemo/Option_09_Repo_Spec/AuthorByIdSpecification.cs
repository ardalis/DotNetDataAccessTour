using Microsoft.EntityFrameworkCore;
using WebDataDemo.Model;

namespace WebDataDemo.Option_09_Repo_Spec;

public class AuthorByIdSpecification : AuthorSpecification
{
    public AuthorByIdSpecification(int id)
    {
        Predicate = Author => Author.Id == id;
        IncludeExpression = entity => entity.Include(a => a.Courses).ThenInclude(ca => ca.Course);
    }
}
