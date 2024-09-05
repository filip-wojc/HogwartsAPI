using FluentValidation;
using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;

namespace HogwartsAPI.Dtos.HomeworkSubmissionsValidators
{
    public class ModifyHomeworkSubmissionValidator : AbstractValidator<ModifyHomeworkSubmissionDto>
    {
        public ModifyHomeworkSubmissionValidator()
        {
            RuleFor(h => h.Grade).GreaterThanOrEqualTo(1).LessThanOrEqualTo(6);
        }
    }
}
