using Ardalis.Specification;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public class AuthorByIdWithCoursesSpec : Specification<Author>, ISingleResultSpecification<Author>
{
  public AuthorByIdWithCoursesSpec(int id)
  {
    Query
        .Where(a => a.Id == id)
        .Include(author => author.Courses)
        .ThenInclude(ca => ca.Course);

    Query.EnableCache(nameof(AuthorByIdWithCoursesSpec), id);

  }
}
