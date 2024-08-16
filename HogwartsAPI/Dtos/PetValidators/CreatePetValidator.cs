using FluentValidation;
using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Enums;

namespace HogwartsAPI.Dtos.PetValidators
{
    public class CreatePetValidator : AbstractValidator<CreatePetDto>
    {
        private readonly HogwartDbContext _context;
        public CreatePetValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Type).NotEmpty().Must(
                (pet, x) => TypeExists(pet.Type)
                ).WithMessage("That pet type does not exist");
            RuleFor(p => p.StudentId).NotEmpty().Must(
               (student, x) => StudentExists(student.StudentId)
               ).WithMessage($"That id does not exist");
        }

        private bool StudentExists(int studentId)
        {
            return _context.Students.Any(s => s.Id == studentId);   
        }

        private bool TypeExists(string type)
        {
            type = char.ToUpper(type[0]) + type.Substring(1);
            if (!Enum.IsDefined(typeof(PetType), type))
            {
                return false;
            }
            return true;
        }
    }
}
