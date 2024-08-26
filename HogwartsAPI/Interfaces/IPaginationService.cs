using HogwartsAPI.Tools;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HogwartsAPI.Interfaces
{
    public interface IPaginationService<T> where T : class
    {
        public PageResult<T> GetPaginatedResult(IPaginateQuery query, IEnumerable<T> allEntities);
    }
}
