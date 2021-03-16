using System.Collections.Generic;

namespace WebDataDemo.Model
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CourseAuthor> Courses { get; set; } = new List<CourseAuthor>();
    }

    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        public List<CourseAuthor> Authors { get; set; } = new List<CourseAuthor>();
    }

    public class CourseAuthor
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CourseId { get; set; }
        public int RoyaltyPercentage { get; set; }
    }
}
