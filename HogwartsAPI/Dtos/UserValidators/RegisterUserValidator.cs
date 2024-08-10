using FluentValidation;


using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.UserValidators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(HogwartDbContext db)
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
            RuleFor(u => u.ConfirmPassword).Equal(u => u.Password);

            RuleFor(u => u.Email)
                .Custom((value, context) =>
                {
                    var mail = db.Users.Any(u => u.Email == value);
                    if (mail)
                    {
                        context.AddFailure("Email", "Email is taken");
                    }
                });
        }
    }
}
