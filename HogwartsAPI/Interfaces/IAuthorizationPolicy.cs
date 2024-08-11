namespace HogwartsAPI.Interfaces
{
    public interface IAuthorizationPolicy
    {
        void AddAuthorizationPolicy(IServiceCollection services);
    }
}
