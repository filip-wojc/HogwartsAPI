namespace HogwartsAPI.Dtos
{
    public class CourseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DifficultyLevel { get; set; }
        public DateTime Date { get; set; }
        public string?  TeacherName { get; set; }
        public IEnumerable<string>? StudentsNames { get; set; }
    }
}
