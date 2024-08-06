using HogwartsAPI.Enums;

namespace HogwartsAPI.Dtos.StudentDtos
{
    public class StudentsDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int SchoolYear { get; set; }
        public string HouseName { get; set; }
        public string WandCore {  get; set; }
        public IEnumerable<string>? PetNames { get; set; }
        public IEnumerable<string>? CourseNames { get; set; }
    }
}
