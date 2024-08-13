using HogwartsAPI.Enums;
using HogwartsAPI.Interfaces;

namespace HogwartsAPI.Entities
{
    public class Pet : IResource
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public PetType Type { get; set; }
        public bool HasMagicAbility { get; set; }
        public virtual Student? Student { get; set; }
        public int StudentId { get; set; }
        public virtual User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
    }
}
