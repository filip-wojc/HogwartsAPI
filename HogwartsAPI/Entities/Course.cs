namespace HogwartsAPI.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DifficultyLevel { get; set; }
        public DateTime Date { get; set; }
        public virtual IEnumerable<Student>? Students { get; set; }
        public Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }
        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
    }
}
