using FluentValidation;
using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Dtos.TeacherValidators
{
    public class CreateTeacherValidator : AbstractValidator<CreateTeacherDto>
    {
        private readonly HogwartDbContext _context;
        public CreateTeacherValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(t => t.Name).NotEmpty();
            RuleFor(t => t.Surname).NotEmpty();
            RuleFor(t => t.DateOfBirth).NotEmpty();
            RuleFor(t => t.WandId).NotEmpty().Must(
                (wand, x) => WandExists(wand.WandId)
                ).WithMessage($"That id does not exist");
        }

        private bool WandExists(int wandId)
        {
            return _context.Wands.Any(w => w.Id == wandId);
        }
    }
}
