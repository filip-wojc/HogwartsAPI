using HogwartsAPI.Dtos.StudentDtos;
using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class StudentModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGetEntitiesService<StudentDto>, StudentService>();
            services.AddScoped<IAddEntitiesService<CreateStudentDto>, StudentService>();
            services.AddScoped<IDeleteEntitiesService<Student>, StudentService>();
            services.AddScoped<IModifyEntitiesService<ModifyStudentDto>, StudentService>();
            services.AddScoped<IPaginationService<StudentDto>, StudentPaginationService>();
        }
    }
}
