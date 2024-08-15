namespace HogwartsAPI.Dtos.HouseDtos
{
    public class HouseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int TrophyCount { get; set; }
        public int StudentsCount { get; set; }
        public string? TeacherName { get; set; }
    }
}
