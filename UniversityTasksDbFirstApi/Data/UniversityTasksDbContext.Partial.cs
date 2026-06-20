using Microsoft.EntityFrameworkCore;
using UniversityTasksDbFirstApi.Models;

namespace UniversityTasksDbFirstApi.Data
{
    public partial class UniversityTasksDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .Property(a => a.IsPublished)
                .HasDefaultValue(false, "DF_Assignments_IsPublished");

            modelBuilder.Entity<Course>()
           .ToTable(t => t.HasCheckConstraint(
               "CK_Courses_Credits",
               "[Credits] BETWEEN 1 AND 10"));

            modelBuilder.Entity<Enrollment>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Enrollments_Status",
                    "[Status] IN (N'Active', N'Completed', N'Dropped')"));

            modelBuilder.Entity<Assignment>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Assignments_MaxPoints",
                    "[MaxPoints] > 0"));

            modelBuilder.Entity<Assignment>()
                .Property(e => e.IsPublished)
                .HasDefaultValue(false, "DF_Assignments_IsPublished");

            modelBuilder.Entity<Submission>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Submissions_Status",
                    "[Status] IN (N'Submitted', N'Late', N'Graded')"));

            modelBuilder.Entity<Submission>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Submissions_Score",
                    "[Score] IS NULL OR [Score] >= 0"));
        }
    }
}
