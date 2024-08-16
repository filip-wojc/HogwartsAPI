namespace HogwartsAPI.Dtos.TeacherDtos
{
    public class CreateTeacherDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int WandId { get; set; }
    }
}
