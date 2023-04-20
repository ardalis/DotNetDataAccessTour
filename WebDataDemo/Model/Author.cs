namespace WebDataDemo.Model;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<CourseAuthor> Courses { get; set; } = new List<CourseAuthor>();
}
