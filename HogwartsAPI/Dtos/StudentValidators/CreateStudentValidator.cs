using FluentValidation;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Dtos.StudentValidators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
    {
        private readonly HogwartDbContext _context;
        public CreateStudentValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.Surname).NotEmpty();
            RuleFor(s => s.DateOfBirth).NotEmpty();
            RuleFor(s => s.SchoolYear).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(7);
            RuleFor(s => s.WandId).NotEmpty().Must(
                (wand, x) => WandExists(wand.WandId)
                ).WithMessage($"That id does not exist");
            RuleFor(s => s.HouseId).NotEmpty().Must(
                (house, x) => HouseExists(house.HouseId)
                ).WithMessage($"That id does not exist");

        }

        private bool WandExists(int wandId)
        {
            return _context.Wands.Any(w => w.Id == wandId);
        }

        private bool HouseExists(int houseId)
        {
            return _context.Houses.Any(h => h.Id == houseId);
        }
    }
}
