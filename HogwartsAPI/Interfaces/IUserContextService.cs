using System.Security.Claims;

namespace HogwartsAPI.Interfaces
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
    }
}
