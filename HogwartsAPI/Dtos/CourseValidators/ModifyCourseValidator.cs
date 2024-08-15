using FluentValidation;
using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.CourseValidators
{
    public class ModifyCourseValidator : AbstractValidator<ModifyCourseDto>
    {
        private readonly HogwartDbContext _context;
        public ModifyCourseValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(c => c.DifficultyLevel).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
            RuleFor(c => c.TeacherId).Must(
                (teacher, x) => TeacherExists(teacher.TeacherId)
                ).WithMessage($"That id does not exist");
            RuleFor(w => w.TeacherId).Must(
                (teacher, x) => !TeacherHasCourse(teacher.TeacherId)
                ).WithMessage($"Teacher with that id has already signed up to the course");
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
        private bool TeacherHasCourse(int? teacherId)
        {
            if (teacherId == null)
            {
                return false;
            }
            var isCourse = _context.Courses.Any(c => c.TeacherId == teacherId);
            return isCourse;
        }
    }
    
}
