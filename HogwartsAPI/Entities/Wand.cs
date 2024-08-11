using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HogwartsAPI.Entities
{
    public class Wand
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        [Range(9,14)]
        public double Length { get; set; }
        public string? WoodType { get; set; }
        public string? Color { get; set; }
        public Core? Core { get; set; }
        public int CoreId { get; set; }
        public virtual IEnumerable<Student>? StudentOwners { get; set; }
        public virtual IEnumerable<Teacher>? TeacherOwners { get; set; }
        public virtual User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
    }
}
