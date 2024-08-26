using HogwartsAPI.Tools;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HogwartsAPI.Interfaces
{
    public interface IPaginationService<T> where T : class
    {
        public PageResult<T> GetPaginatedResult(PaginateQuery query, IEnumerable<T> allEntities);
    }
}
