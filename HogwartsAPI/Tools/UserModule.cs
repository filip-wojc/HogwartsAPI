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
            services.AddScoped<ILoginService, AccountService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IModifyEntitiesService<ChangeUserRoleDto>, AccountService>();
            services.AddScoped<IModifyEntitiesService<ModifyUserDto>, AccountService>();
        }
    }
}
