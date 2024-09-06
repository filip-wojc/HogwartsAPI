using FluentValidation;
using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.HomeworkValidators
{
    public class HomeworkResultValidator : AbstractValidator<HomeworkResultDto>
    {
        private readonly HogwartDbContext _context;
        public HomeworkResultValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(h => h.FullName).NotEmpty();
            RuleFor(h => h.Title).NotEmpty();
            RuleFor(h => h.Content).NotEmpty();
            RuleFor(h => h.HomeworkId).NotEmpty().Must(
                (h, x) => HomeworkExists(h.HomeworkId)
                ).WithMessage($"That homework id does not exist");
        }

        private bool HomeworkExists(int homeworkId)
        {
            return _context.Homeworks.Any(h => h.Id == homeworkId);
        }
    }
}
