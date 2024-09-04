using FluentValidation;
using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;
using HogwartsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace HogwartsAPI.Dtos.HomeworkSubmissionsValidators
{
    public class CreateHomeworkSubmissionValidator : AbstractValidator<CreateHomeworkSubmissionDto>
    {
        private readonly HogwartDbContext _context;
        public CreateHomeworkSubmissionValidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(h => h.Feedback).NotEmpty();
            RuleFor(h => h.Grade).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(6);
            RuleFor(h => h.HomeworkId).Must(
               (h, x) => HomeworkExists(h.HomeworkId)
               ).WithMessage($"That homework id does not exist");
            RuleFor(h => h.StudentId).Must(
               (h, x) => StudentExists(h.StudentId)
               ).WithMessage($"That student id does not exist");

            When(h => HomeworkExists(h.HomeworkId) && StudentExists(h.StudentId), () =>
            {
                RuleFor(h => h).Custom((h, context) =>
                {
                    if (!StudentHasHomework(h.StudentId, h.HomeworkId))
                    {
                        context.AddFailure("StudentId, HomeworkId","Student doesn't have this homework");
                    }
                });
            });
            
              
        }
        private bool HomeworkExists(int? homeworkId)
        {
            return _context.Homeworks.Any(h => h.Id == homeworkId);
        }
        private bool StudentExists(int? studentId)
        {
            return _context.Students.Any(s => s.Id == studentId);
        }

        private bool StudentHasHomework(int studentId, int homeworkId)
        {
            var homework = _context.Homeworks.Include(h => h.Course).FirstOrDefault(h => h.Id == homeworkId);
            var student = _context.Students.Include(s => s.Courses).FirstOrDefault(s => s.Id == studentId);
            if (student.Courses.Any(c => c.Id == homework.Course.Id))
            {
                return true;
            }
            return false;
        }
    }
}
