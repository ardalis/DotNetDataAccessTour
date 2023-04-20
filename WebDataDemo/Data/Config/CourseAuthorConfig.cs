using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebDataDemo.Model;

namespace WebDataDemo.Data.Config;

public class CourseAuthorConfig : IEntityTypeConfiguration<CourseAuthor>
{
    public void Configure(EntityTypeBuilder<CourseAuthor> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasData(new { Id = 1, AuthorId = 1, CourseId = 1, RoyaltyPercentage = 100 });
        builder.HasData(new { Id = 2, AuthorId = 1, CourseId = 2, RoyaltyPercentage = 100 });
        builder.HasData(new { Id = 3, AuthorId = 1, CourseId = 3, RoyaltyPercentage = 50 });
        builder.HasData(new { Id = 4, AuthorId = 2, CourseId = 3, RoyaltyPercentage = 50 });
    }
}
