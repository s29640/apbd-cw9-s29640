using global::UniversityTasksDbFirstApi.Data;
using global::UniversityTasksDbFirstApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniversityTasksDbFirstApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly UniversityTasksDbContext _context;

        public CoursesController(UniversityTasksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
            [FromQuery] bool activeOnly = true)
        {
            var query = _context.Courses.AsNoTracking();

            if (activeOnly)
            {
                query = query.Where(c => c.IsActive);
            }

            var courses = await query
                .OrderBy(c => c.Code)
                .Select(c => new CourseDto
                {
                    IdCourse = c.CourseId,
                    Code = c.Code,
                    Name = c.Name,
                    Ects = c.Credits,
                    AssignmentCount = c.Assignments.Count
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{idCourse:int}/assignments")]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments(
            int idCourse,
            [FromQuery] bool publishedOnly = true)
        {
            var courseExists = await _context.Courses
                .AsNoTracking()
                .AnyAsync(c => c.CourseId == idCourse);

            if (!courseExists)
            {
                return NotFound();
            }

            var query = _context.Assignments
                .AsNoTracking()
                .Where(a => a.CourseId == idCourse);

            if (publishedOnly)
            {
                query = query.Where(a => a.IsPublished);
            }

            var assignments = await query
                .OrderBy(a => a.DueDate)
                .Select(a => new AssignmentDto
                {
                    IdAssignment = a.AssignmentId,
                    Title = a.Title,
                    DueDate = a.DueDate,
                    MaxPoints = a.MaxPoints,
                    IsPublished = a.IsPublished,
                    SubmissionCount = a.Submissions.Count
                })
                .ToListAsync();

            return Ok(assignments);
        }
    }
}
