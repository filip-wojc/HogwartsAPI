using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class WandModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGetEntitiesService<WandDto>, WandService>();
            services.AddScoped<IAddEntitiesService<CreateWandDto>, WandService>();
            services.AddScoped<IDeleteEntitiesService<Wand>, WandService>();
            services.AddScoped<IModifyEntitiesService<ModifyWandDto>, WandService>();
            services.AddScoped<IPaginationService<WandDto>, WandPaginationService>();
        }
    }
}
