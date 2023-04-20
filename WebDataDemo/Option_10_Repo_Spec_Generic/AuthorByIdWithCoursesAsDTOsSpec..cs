using Ardalis.Specification;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public class AuthorByIdWithCoursesAsDTOsSpec : Specification<Author, AuthorWithCoursesDTO>,
                                                ISingleResultSpecification
{
  public AuthorByIdWithCoursesAsDTOsSpec(int id)
  {
    Query
        .Where(a => a.Id == id)
        .Include(author => author.Courses)
        .ThenInclude(ca => ca.Course);
    Query
        .Select(a => new AuthorWithCoursesDTO
        {
          Id = a.Id,
          Name = a.Name,
          Courses = a.Courses.Select(c => new CourseDTO
          {
            Id = c.Id,
            Title = c.Course.Title,
            AuthorId = a.Id,
            RoyaltyPercentage = c.RoyaltyPercentage
          }).ToList()
        });

    Query.EnableCache(nameof(AuthorByIdWithCoursesAsDTOsSpec), id);
  }
}
