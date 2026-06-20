using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityTasksDbFirstApi.Data;
using UniversityTasksDbFirstApi.DTOs;

namespace UniversityTasksDbFirstApi.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityTasksDbContext _context;

        public StudentsController(UniversityTasksDbContext context)
        {
            _context = context;
        }

        [HttpGet("{idStudent:int}/dashboard")]
        public async Task<ActionResult<StudentDashboardDto>> GetDashboard(int idStudent)
        {
            var dashboard = await _context.Students
                .AsNoTracking()
                .Where(s => s.StudentId == idStudent)
                .Select(s => new StudentDashboardDto
                {
                    IdStudent = s.StudentId,
                    IndexNumber = s.IndexNumber,
                    FullName = s.FirstName + " " + s.LastName,
                    IsActive = s.IsActive,
                    Enrollments = s.Enrollments
                        .OrderBy(e => e.Course.Code)
                        .Select(e => e.Course.Code + " - " + e.Course.Name + " (" + e.Status + ")")
                        .ToList(),
                    Submissions = s.Submissions
                        .OrderByDescending(sub => sub.SubmittedAt)
                        .Select(sub => new SubmissionDto
                        {
                            IdSubmission = sub.SubmissionId,
                            Student = s.FirstName + " " + s.LastName,
                            Assignment = sub.Assignment.Title,
                            RepositoryUrl = sub.RepositoryUrl,
                            Status = sub.Status,
                            Points = sub.Score,
                            Comment = sub.Feedback
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (dashboard is null)
            {
                return NotFound();
            }

            return Ok(dashboard);
        }
    }
}