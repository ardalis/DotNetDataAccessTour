namespace WebDataDemo.Model;

public class Author
{
  public int Id { get; set; }
  public string Name { get; set; }
  public byte[] Data { get; set; } // could be some large amount of data

  public List<CourseAuthor> Courses { get; set; } = new List<CourseAuthor>();
}
