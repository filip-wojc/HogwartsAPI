using HogwartsAPI.Enums;

namespace HogwartsAPI.Dtos.PetDtos
{
    public class PetDto
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool HasMagicAbility { get; set; }
        public string? OwnerName { get; set; }
    }
}
