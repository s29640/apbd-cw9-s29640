namespace UniversityTasksDbFirstApi.DTOs
{
    public class SubmissionDto
    {
        public int IdSubmission { get; set; }
        public string Student { get; set; } = null!;
        public string Assignment { get; set; } = null!;
        public string RepositoryUrl { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Points { get; set; }
        public string? Comment { get; set; }
    }
}
