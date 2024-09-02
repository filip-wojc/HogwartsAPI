using FluentValidation;
using HogwartsAPI.Dtos.HomeworksDto;

namespace HogwartsAPI.Dtos.HomeworkValidators
{
    public class HomeworkResultValidator : AbstractValidator<HomeworkResultDto>
    {
        public HomeworkResultValidator()
        {
            RuleFor(h => h.FullName).NotEmpty();
            RuleFor(h => h.Title).NotEmpty();
            RuleFor(h => h.Content).NotEmpty(); 
        }
    }
}
