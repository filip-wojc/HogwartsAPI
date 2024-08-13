namespace HogwartsAPI.Interfaces
{
    public interface IManyToManyRelationAddService<TParrent, TChild> where TParrent : class where TChild : class
    {
        Task Create(int parrentId, int childId);
    }
}
