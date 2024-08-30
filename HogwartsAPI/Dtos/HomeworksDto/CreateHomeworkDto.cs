namespace HogwartsAPI.Dtos.HomeworksDto
{
    public class CreateHomeworkDto
    {
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
    }
}
