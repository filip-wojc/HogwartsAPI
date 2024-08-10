using HogwartsAPI.Dtos.UserDtos;

namespace HogwartsAPI.Interfaces
{
    public interface ILoginService
    {
        Task<string> GenerateJwt(LoginUserDto dto);
    }
}
