using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class HomeworkModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IManyToManyRelationGetService<Course, HomeworkDto>, HomeworkService>();
            services.AddScoped<IAddEntitiesService<CreateHomeworkDto>, HomeworkService>();
            services.AddScoped<IManyToManyRelationDeleteService<Course, Homework>, HomeworkService>();
            services.AddScoped<IModifyEntitiesService<ModifyHomeworkDto>, HomeworkService>();
            services.AddScoped<IHomeworkFileService<HomeworkResultDto>, HomeworkSenderService>();
        }
    }
}
