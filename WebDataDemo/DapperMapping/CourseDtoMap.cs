using Dapper.FluentMap.Mapping;
using WebDataDemo.Dtos;

namespace WebDataDemo.DapperMapping;

public class CourseDtoMap : EntityMap<CourseDTO>
{
  public CourseDtoMap()
  {
    Map(c => c.Id)
        .ToColumn("CourseId");
  }
}
