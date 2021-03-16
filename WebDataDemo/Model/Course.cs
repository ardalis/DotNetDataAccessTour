using System.Collections.Generic;

namespace WebDataDemo.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        public List<CourseAuthor> Authors { get; set; } = new List<CourseAuthor>();
    }
}
