using HogwartsAPI.Enums;

namespace HogwartsAPI.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public PetType Type { get; set; }
        public bool HasMagicAbility { get; set; }
        public virtual Student? Student { get; set; }
        public int StudentId { get; set; }
    }
}
