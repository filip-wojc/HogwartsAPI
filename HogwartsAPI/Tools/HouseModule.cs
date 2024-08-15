using HogwartsAPI.Dtos.HouseDtos;
using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class HouseModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGetEntitiesService<HouseDto>, HouseService>();
            services.AddScoped<IModifyEntitiesService<ModifyHouseDto>, HouseService>();
            services.AddScoped<IManyToManyRelationGetService<House, StudentDto>, HouseService>();
        }
    }
}
