using Ardalis.Specification;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public class AllAuthorsAsDTOsSpec2 : Specification<Author>
{
    public AllAuthorsAsDTOsSpec2()
    {
        Query.EnableCache(nameof(AllAuthorsAsDTOsSpec2));
    }
}
