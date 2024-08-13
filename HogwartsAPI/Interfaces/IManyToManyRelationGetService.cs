namespace HogwartsAPI.Interfaces
{
    public interface IManyToManyRelationGetService<TParrent, TChild> where TParrent : class where TChild : class
    {
        Task<IEnumerable<TChild>> GetAllChildren(int parrentId);
        Task<TChild> GetChildById(int parrentId, int childId);
    }
}
