using Ardalis.Specification;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public class AllAuthorsAsDTOsSpec : Specification<Author, AuthorDTO>
{
    public AllAuthorsAsDTOsSpec()
    {
        Query
            .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name });

        Query.EnableCache(nameof(AllAuthorsAsDTOsSpec));
    }
}
