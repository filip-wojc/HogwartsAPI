using HogwartsAPI.Dtos.EventsDtos;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class EventsModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IFileService<EventUploadDto>, EventsService>();
        }
    }
}
