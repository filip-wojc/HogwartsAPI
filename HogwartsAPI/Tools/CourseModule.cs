using HogwartsAPI.Dtos;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class CourseModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGetEntitiesService<CourseDto>, CourseService>();
        }
    }
}
