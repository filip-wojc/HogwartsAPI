using HogwartsAPI.Dtos.CourseDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class CourseModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGetEntitiesService<CourseDto>, CourseService>();
            services.AddScoped<IAddEntitiesService<CreateCourseDto>, CourseService>();
            services.AddScoped<IModifyEntitiesService<ModifyCourseDto>, CourseService>();
            services.AddScoped<IDeleteEntitiesService<Course>, CourseService>();
            services.AddScoped<IPaginationService<CourseDto>, CoursePaginationService>();
        }
    }
}
