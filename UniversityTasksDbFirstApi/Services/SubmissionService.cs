using global::UniversityTasksDbFirstApi.Data;
using global::UniversityTasksDbFirstApi.DTOs;
using global::UniversityTasksDbFirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace UniversityTasksDbFirstApi.Services
{
    public class SubmissionService
    {
        private readonly UniversityTasksDbContext _context;

        public SubmissionService(UniversityTasksDbContext context)
        {
            _context = context;
        }

        public async Task<(int StatusCode, SubmissionDto? Data, string? Error)> CreateAsync(
            CreateSubmissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RepositoryUrl) ||
                !dto.RepositoryUrl.StartsWith("https://"))
            {
                return (400, null, "RepositoryUrl must start with https://.");
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == dto.IdStudent);

            if (student is null)
            {
                return (404, null, "Student not found.");
            }

            if (!student.IsActive)
            {
                return (400, null, "Student is not active.");
            }

            var assignment = await _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.AssignmentId == dto.IdAssignment);

            if (assignment is null)
            {
                return (404, null, "Assignment not found.");
            }

            if (!assignment.IsPublished)
            {
                return (400, null, "Assignment is not published.");
            }

            var isEnrolled = await _context.Enrollments.AnyAsync(e =>
                e.StudentId == dto.IdStudent &&
                e.CourseId == assignment.CourseId &&
                (e.Status == "Active" || e.Status == "Completed"));

            if (!isEnrolled)
            {
                return (400, null, "Student is not enrolled in this course.");
            }

            var alreadySubmitted = await _context.Submissions.AnyAsync(s =>
                s.StudentId == dto.IdStudent &&
                s.AssignmentId == dto.IdAssignment);

            if (alreadySubmitted)
            {
                return (409, null, "Student already submitted this assignment.");
            }

            var now = DateTime.UtcNow;

            var submission = new Submission
            {
                StudentId = dto.IdStudent,
                AssignmentId = dto.IdAssignment,
                RepositoryUrl = dto.RepositoryUrl,
                SubmittedAt = now,
                Status = assignment.IsOverdue(now) ? "Late" : "Submitted"
            };

            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();

            var result = new SubmissionDto
            {
                IdSubmission = submission.SubmissionId,
                Student = student.FirstName + " " + student.LastName,
                Assignment = assignment.Title,
                RepositoryUrl = submission.RepositoryUrl,
                Status = submission.Status,
                Points = submission.Score,
                Comment = submission.Feedback
            };

            return (201, result, null);
        }

        public async Task<(int StatusCode, SubmissionDto? Data, string? Error)> GradeAsync(
            int idSubmission,
            GradeSubmissionDto dto)
        {
            if (dto.Points < 0)
            {
                return (400, null, "Points cannot be less than 0.");
            }

            var submission = await _context.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.SubmissionId == idSubmission);

            if (submission is null)
            {
                return (404, null, "Submission not found.");
            }

            if (dto.Points > submission.Assignment.MaxPoints)
            {
                return (400, null, "Points cannot be greater than assignment MaxPoints.");
            }

            submission.Score = dto.Points;
            submission.Feedback = dto.Comment;
            submission.Status = "Graded";

            await _context.SaveChangesAsync();

            return (200, new SubmissionDto
            {
                IdSubmission = submission.SubmissionId,
                Student = submission.Student.FirstName + " " + submission.Student.LastName,
                Assignment = submission.Assignment.Title,
                RepositoryUrl = submission.RepositoryUrl,
                Status = submission.Status,
                Points = submission.Score,
                Comment = submission.Feedback
            }, null);
        }

        public async Task<(int StatusCode, string? Error)> DeleteAsync(int idSubmission)
        {
            var submission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.SubmissionId == idSubmission);

            if (submission is null)
            {
                return (404, "Submission not found.");
            }

            if (submission.Status == "Graded")
            {
                return (400, "Graded submission cannot be deleted.");
            }

            _context.Submissions.Remove(submission);
            await _context.SaveChangesAsync();

            return (204, null);
        }
    }
}
