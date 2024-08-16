using HogwartsAPI.Dtos.PetDtos;
using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Services;

namespace HogwartsAPI.Tools
{
    public class PetModule : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGetEntitiesService<PetDto>, PetService>();
            services.AddScoped<IAddEntitiesService<CreatePetDto>, PetService>();
            services.AddScoped<IDeleteEntitiesService<Pet>, PetService>();
            services.AddScoped<IManyToManyRelationGetService<Student, PetDto>, PetService>();
        }
    }
}
