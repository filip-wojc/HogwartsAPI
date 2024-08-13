using FluentValidation;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Dtos.StudentValidators
{
    public class ModifyStudentWalidator : AbstractValidator<ModifyStudentDto>
    {
        private readonly HogwartDbContext _context;
        public ModifyStudentWalidator(HogwartDbContext context)
        {
            _context = context;
            RuleFor(s => s.SchoolYear).GreaterThanOrEqualTo(1).LessThanOrEqualTo(7);
            RuleFor(s => s.WandId).Must(
                (wand, x) => WandExists(wand.WandId)
                ).WithMessage($"That id does not exist");
        }
        private bool WandExists(int? wandId)
        {
            var isWand = _context.Wands.Any(w => w.Id == wandId);
            if (isWand || wandId == null)
            {
                return true;
            }
            return false;
        }
    }
}
