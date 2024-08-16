using FluentValidation;
using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Dtos.TeacherValidators
{
    public class ModifyTeacherValidator : AbstractValidator<ModifyTeacherDto>
    {
        private readonly HogwartDbContext _context;
        public ModifyTeacherValidator(HogwartDbContext context)
        {
            _context = context;

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
