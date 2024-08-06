namespace HogwartsAPI.Dtos.StudentDtos
{
    public class CreateStudentDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int WandId { get; set; }
        public int SchoolYear {  get; set; }
        public int HouseId { get; set; }
    }
}
