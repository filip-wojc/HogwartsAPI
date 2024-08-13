namespace HogwartsAPI.Dtos.CourseDtos
{
    public class CreateCourseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DifficultyLevel { get; set; }
        public int TeacherId { get; set; }
    }
}
