using HogwartsAPI.Dtos.StudentDtos;

namespace HogwartsAPI.Interfaces
{
    public interface IAddEntitiesService<T> where T : class
    {
        Task<int> Create(T dto);
    }
}
