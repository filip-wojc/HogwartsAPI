using FluentValidation;
using HogwartsAPI.Dtos.EventsDtos;

namespace HogwartsAPI.Dtos.EventsValidators
{
    public class EventUploadValidator : AbstractValidator<EventUploadDto>
    {
        public EventUploadValidator()
        {
            RuleFor(e => e.FullName).NotEmpty();
            RuleFor(e => e.Place).NotEmpty();
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Date).NotEmpty();
            RuleFor(e => e.Title).NotEmpty();
        }
    }
}
