namespace UniversityTasksDbFirstApi.DTOs
{
    public class CourseDto
    {
        public int IdCourse { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Ects { get; set; }
        public int AssignmentCount { get; set; }
    }
}
