namespace HogwartsAPI.Interfaces
{
    public interface IModifyEntitiesService<T> where T : class
    {
        Task Modify(int id, T dto);
    }
}
