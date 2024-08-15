using FluentValidation;
using HogwartsAPI.Dtos.HouseDtos;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.HouseValidators
{
    public class ModifyHouseValidator : AbstractValidator<ModifyHouseDto>
    {
        private readonly HogwartDbContext _context;
        public ModifyHouseValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(h => h.TeacherId).Must(
                (teacher, x) => TeacherExists(teacher.TeacherId)
                ).WithMessage($"That id does not exist");

            RuleFor(h => h.TeacherId).Must(
                (teacher, x) => !TeacherHasHouse(teacher.TeacherId)
                ).WithMessage($"That teacher is alreadyt a caretaker of the house");
        }

        private bool TeacherExists(int? teacherId)
        {
            var isTeacher = _context.Teachers.Any(t => t.Id == teacherId);
            if (isTeacher || teacherId == null)
            {
                return true;
            }
            return false;
        }

        private bool TeacherHasHouse(int? teacherId)
        {
            if (teacherId == null)
            {
                return false;
            }
            var isTeacher = _context.Houses.Any(h => h.TeacherId == teacherId);
            return isTeacher;
        }
    }
}
