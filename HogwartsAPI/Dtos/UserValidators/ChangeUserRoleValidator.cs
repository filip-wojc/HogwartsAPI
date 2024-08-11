using FluentValidation;
using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HogwartsAPI.Dtos.UserValidators
{
    public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleDto>
    {
        private readonly HogwartDbContext _db;
        public ChangeUserRoleValidator(HogwartDbContext db)
        {
            _db = db;

            RuleFor(u => u.RoleId).NotEmpty();
            RuleFor(s => s.RoleId).Must(
                (role, x) => RoleExists(role.RoleId) 
                ).WithMessage($"That role id does not exist");
            
        }

        private bool RoleExists(int? roleId)
        {
            return _db.Roles.Any(r => r.Id == roleId);
        }
    }
}
