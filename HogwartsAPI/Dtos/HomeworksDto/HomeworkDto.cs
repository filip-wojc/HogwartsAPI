namespace HogwartsAPI.Dtos.HomeworksDto
{
    public class HomeworkDto
    {
        public string? CourseName { get; set; }
        public string? TeacherName { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
