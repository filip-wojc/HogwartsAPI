using HogwartsAPI.Dtos.StudentDtos;

namespace HogwartsAPI.Interfaces
{
    public interface IGetEntitiesService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
    }
}
