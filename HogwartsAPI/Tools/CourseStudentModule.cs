using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class CourseStudentModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IManyToManyRelationAddService<Course, Student>, CourseStudentsService>();
            services.AddScoped<IManyToManyRelationDeleteService<Course, Student>, CourseStudentsService>();
            services.AddScoped<IManyToManyRelationGetService<Course, StudentDto>, CourseStudentsService>();
        }
    }
}
