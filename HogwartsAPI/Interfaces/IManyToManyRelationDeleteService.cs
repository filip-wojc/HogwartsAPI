namespace HogwartsAPI.Interfaces
{
    public interface IManyToManyRelationDeleteService<TParrent, TChild> where TParrent : class where TChild : class
    {
        Task DeleteChild(int parrentId, int childId);
        Task DeleteAllChildren(int parrentId);
    }
}
