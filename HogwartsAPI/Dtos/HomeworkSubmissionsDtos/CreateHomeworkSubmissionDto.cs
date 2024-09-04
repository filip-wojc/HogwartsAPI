namespace HogwartsAPI.Dtos.HomeworkSubmissionsDtos
{
    public class CreateHomeworkSubmissionDto
    {
        public int HomeworkId { get; set; }
        public int StudentId { get; set; }
        public int Grade { get; set; }
        public string? Feedback { get; set; }
    }
}
