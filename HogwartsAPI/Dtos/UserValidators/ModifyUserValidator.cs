using FluentValidation;
using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Entities;
using System.Text.RegularExpressions;

namespace HogwartsAPI.Dtos.UserValidators
{
    public class ModifyUserValidator : AbstractValidator<ModifyUserDto>
    {
        private readonly HogwartDbContext _db;
        public ModifyUserValidator(HogwartDbContext db)
        {
            _db = db;

            RuleFor(u => u.ConfirmPassword).Equal(u => u.Password);
            RuleFor(u => u.Username)
               .Custom((value, context) =>
               {
                   var username = _db.Users.Any(u => u.Username == value);
                   if (username)
                   {
                       context.AddFailure("Username", "Username is taken or this is your current username");
                   }
               });
            RuleFor(u => u.Password).MinimumLength(6)
                .Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }

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
