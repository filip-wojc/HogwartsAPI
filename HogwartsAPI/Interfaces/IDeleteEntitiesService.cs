namespace HogwartsAPI.Interfaces
{
    public interface IDeleteEntitiesService<T> where T : class
    {
        Task Delete(int id);
    }
}
