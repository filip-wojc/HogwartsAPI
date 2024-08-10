using FluentValidation;
using HogwartsAPI.Dtos.UserDtos;

namespace HogwartsAPI.Dtos.UserValidators
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty();
        }
    }
}
