namespace HogwartsAPI.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Wand? Wand { get; set; }
        public int WandId { get; set; }
        public virtual Course? Course { get; set; }
    }
}
