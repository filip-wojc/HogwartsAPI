using HogwartsAPI.Enums;

namespace HogwartsAPI.Dtos.PetDtos
{
    public class CreatePetDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool HasMagicAbility { get; set; }
        public int StudentId { get; set; }
    }
}
