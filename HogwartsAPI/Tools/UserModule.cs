using HogwartsAPI.Dtos.UserDtos;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class UserModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAddEntitiesService<RegisterUserDto>, AccountService>();
        }
    }
}
