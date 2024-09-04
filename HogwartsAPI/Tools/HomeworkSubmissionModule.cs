using HogwartsAPI.Dtos.HomeworkSubmissionsDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class HomeworkSubmissionModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IManyToManyRelationGetService<Student, HomeworkSubmissionDto>, HomeworkSubmissionsService>();
            services.AddScoped<IAddEntitiesService<CreateHomeworkSubmissionDto>, HomeworkSubmissionsService>();
        }
    }
}
