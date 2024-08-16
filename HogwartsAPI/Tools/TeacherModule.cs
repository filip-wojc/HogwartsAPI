using HogwartsAPI.Dtos.TeacherDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class TeacherModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped <IGetEntitiesService<TeacherDto>, TeacherService>();
            services.AddScoped <IAddEntitiesService<CreateTeacherDto>, TeacherService>();
            services.AddScoped <IModifyEntitiesService<ModifyTeacherDto>, TeacherService>();
            services.AddScoped <IDeleteEntitiesService<Teacher>, TeacherService>();
        }
    }
}
