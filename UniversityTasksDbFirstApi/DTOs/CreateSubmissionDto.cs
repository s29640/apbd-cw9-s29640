namespace UniversityTasksDbFirstApi.DTOs
{
    public class CreateSubmissionDto
    {
        public int IdAssignment { get; set; }
        public int IdStudent { get; set; }
        public string RepositoryUrl { get; set; } = null!;
    }
}
