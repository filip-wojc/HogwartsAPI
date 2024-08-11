using HogwartsAPI.Interfaces;
using System.Security.Claims;

namespace HogwartsAPI.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _context;
        public UserContextService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public ClaimsPrincipal User => _context.HttpContext?.User;
        public int? UserId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
