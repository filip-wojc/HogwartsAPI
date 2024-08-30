using FluentValidation;
using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.HomeworkValidators
{
    public class CreateHomeworkValidator : AbstractValidator<CreateHomeworkDto>
    {
        private readonly HogwartDbContext _context;
        public CreateHomeworkValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(h => h.Description).NotEmpty();
            RuleFor(h => h.DueDate).NotEmpty();
            RuleFor(h => h.CourseId).Must(
                (course, x) => CourseExists(course.CourseId)
                ).WithMessage($"That id does not exist");
        }

        private bool CourseExists(int? courseId)
        {
            return _context.Courses.Any(c => c.Id == courseId);
        }
    }
}
