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
        public virtual IEnumerable<Pet>? Pets { get; set; }
        public virtual IEnumerable<Course>? Courses { get; set; }
    }
}
