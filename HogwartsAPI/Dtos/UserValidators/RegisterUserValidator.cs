using FluentValidation;


using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Entities;
using System.Text.RegularExpressions;

namespace HogwartsAPI.Dtos.UserValidators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(HogwartDbContext db)
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Username).NotEmpty();
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
            RuleFor(u => u.Username)
                .Custom((value, context) =>
                {
                    var username = db.Users.Any(u => u.Username == value);
                    if (username)
                    {
                        context.AddFailure("Username", "Username is taken");
                    }
                });
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6)
                .Custom((value, context) =>
                {
                    bool isUpperCase = Regex.IsMatch(value, @"[A-Z]");
                    bool isLowerCase = Regex.IsMatch(value, @"[a-z]");
                    bool isDigit = Regex.IsMatch(value, @"[0-9]");
                    bool isSpecialCharacter = Regex.IsMatch(value, @"[\W_]");

                    if (!(isUpperCase && isLowerCase && isDigit && isSpecialCharacter))
                    {
                        context.AddFailure("Password should have at least one upper case and lower case character, one ditit and one special character");
                    }
                });
        }
    }
}
