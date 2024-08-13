using FluentValidation;
using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.CourseValidators
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
    {
        private readonly HogwartDbContext _context;
        public CreateCourseValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
            RuleFor(c => c.DifficultyLevel).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
            RuleFor(w => w.TeacherId).NotEmpty().Must(
                (teacher, x) => TeacherExists(teacher.TeacherId)
                ).WithMessage($"Teacher with that id does not exist");

            RuleFor(w => w.TeacherId).NotEmpty().Must(
                (teacher, x) => !TeacherHasCourse(teacher.TeacherId)
                ).WithMessage($"Teacher with that id has already signed up to the course");
        }

        private bool TeacherExists(int teacherId)
        {
            return _context.Teachers.Any(t => t.Id == teacherId);
        }

        private bool TeacherHasCourse(int teacherId)
        {
            return _context.Courses.Any(c => c.TeacherId == teacherId);
        }
    }
}
