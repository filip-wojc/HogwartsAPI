using HogwartsAPI.Enums;

namespace HogwartsAPI.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Wand Wand { get; set; }
        public int WandId { get; set; }
        public int SchoolYear { get; set; }
        public virtual House House { get; set; }
        public int HouseId { get; set; }
        public virtual ICollection<Pet>? Pets { get; set; } = new List<Pet>();
        public virtual ICollection<Course>? Courses { get; set; } = new List<Course>();
    }
}
