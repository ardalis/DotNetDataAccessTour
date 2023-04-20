namespace WebDataDemo.Dtos;

public class AuthorWithCoursesDTO : AuthorDTO
{
    public List<CourseDTO> Courses { get; set; } = new List<CourseDTO>();
}
