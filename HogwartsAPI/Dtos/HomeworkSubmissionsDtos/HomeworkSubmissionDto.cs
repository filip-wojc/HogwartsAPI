namespace HogwartsAPI.Dtos.HomeworkSubmissionsDtos
{
    public class HomeworkSubmissionDto
    {
        public string? HomeworkDescription { get; set; }
        public string? StudentFullName { get; set; }
        public int Grade { get; set; }
        public string? Feedback { get; set; }
    }
}
